using BL.Contracts.IMapper;
using BL.Contracts.Service.Order.Payment;
using BL.Contracts.Service.Wallet.Customer;
using Common.Enumerations.Payment;
using Common.Enumerations.Wallet.Customer;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Wallet.Customer;
using Serilog;
using Shared.Common.Enumerations.Wallet;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Wallet.Customer;

namespace BL.Services.Wallet.Customer
{
    public class CustomerWalletService : ICustomerWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly IWalletSettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;

        public CustomerWalletService(
            IUnitOfWork unitOfWork,
            IBaseMapper mapper,
            IWalletSettingService settingService,
            IPaymentService paymentService,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _settingService = settingService;
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            return wallet.Balance;
        }

        public async Task<TbCustomerWallet> GetWalletAsync(string userId)
        {
            return await GetOrCreateWalletAsync(userId);
        }

        public async Task<bool> ProcessRefundAsync(string userId, decimal amount, Guid refundId)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");

            return await ProcessTransactionAsync(userId, amount, WalletTransactionType.Refund, WalletTransactionDirection.In, refundId, "Refund");
        }

        public async Task<bool> PayOrderAsync(string userId, decimal amount, Guid orderId)
        {
            _logger.Information("Starting Order Payment for User: {UserId}, Amount: {Amount}, OrderId: {OrderId}", userId, amount, orderId);

            if (amount <= 0) throw new ArgumentException("Amount must be positive");

            // Check Settings
            var settings = await _settingService.GetSettingsAsync();
            if (!settings.IsPaymentEnabled)
            {
                _logger.Warning("Payment failed: Payments globally disabled. User: {UserId}", userId);
                return false;
            }

            // Check Balance
            var wallet = await GetOrCreateWalletAsync(userId);
            if (wallet.Balance < amount)
            {
                _logger.Warning("Payment failed: Insufficient balance. User: {UserId}, Balance: {Balance}, Required: {Amount}", userId, wallet.Balance, amount);
                return false;
            }

            var result = await ProcessTransactionAsync(userId, amount, WalletTransactionType.Payment, WalletTransactionDirection.Out, orderId, "Order");

            if (result) _logger.Information("Order Payment successful. OrderId: {OrderId}", orderId);
            else _logger.Error("Order Payment failed during transaction processing. OrderId: {OrderId}", orderId);

            return result;
        }

        public async Task<IEnumerable<CustomerWalletTransactionsDto>> GetTransactionsAsync(string userId)
        {
            var wallet = await GetOrCreateWalletAsync(userId);

            var allTransactions = await _unitOfWork.TableRepository<TbCustomerWalletTransaction>().GetAsync(x => x.WalletId == wallet.Id);
            return _mapper.MapList<TbCustomerWalletTransaction, CustomerWalletTransactionsDto>(allTransactions);
        }

        public async Task<PaymentResultDto> InitiateChargingRequestAsync(WalletChargingRequestDto request, string userId)
        {
            try
            {
                _logger.Information("Initiating Charging Request for User: {UserId}, Amount: {Amount}", userId, request.Amount);
                await _unitOfWork.BeginTransactionAsync();

                if (request.Amount <= 0) throw new ArgumentException("Amount must be positive");

                // Check Settings
                var settings = await _settingService.GetSettingsAsync();
                if (!settings.IsChargingEnabled)
                {
                    _logger.Warning($"Initiate Charging failed: Charging globally disabled. User: {userId}");
                    throw new InvalidOperationException("Charging is currently disabled.");
                }
                if (request.Amount < settings.MinChargingAmount)
                {
                    _logger.Warning($"Initiate Charging failed: Amount below minimum. User: {userId}, Amount: {request.Amount}, Min: {settings.MinChargingAmount}");
                    throw new ArgumentException($"Amount is less than minimum limit ({settings.MinChargingAmount})");
                }
                if (request.Amount > settings.MaxChargingAmount)
                {
                    _logger.Warning($"Initiate Charging failed: Amount exceeds maximum. User: {userId}, Amount: {request.Amount}, Max: {settings.MaxChargingAmount}");
                    throw new ArgumentException($"Amount exceeds maximum limit ({settings.MaxChargingAmount})");
                }

                // Wallet
                var wallet = await GetOrCreateWalletAsync(userId);
                var transactionId = Guid.NewGuid();

                // Payment request to Gateway
                var paymentRequest = new WalletPaymentProcessRequest
                {
                    Amount = request.Amount,
                    PaymentMethodId = request.PaymentMethodId,
                    ProcessType = PaymentProcessType.WalletTopUp,
                    WalletId = wallet.Id,
                    TransactionId = transactionId
                };

                var proccessPayment = await _paymentService.ProcessPaymentAsync(paymentRequest);
                if (!proccessPayment.Success)
                {
                    _logger.Error($"Charging Payment initiation failed at gateway. User: {userId}, Error: {proccessPayment.Message}");
                    throw new InvalidOperationException("Failed to initiate payment: " + proccessPayment.Message);
                }

                var chargingRequest = new TbWalletChargingRequest
                {
                    Id = transactionId,
                    UserId = userId,
                    Amount = request.Amount,
                    PaymentMethodId = request.PaymentMethodId,
                    Status = WalletTransactionStatus.Pending
                };
                await _unitOfWork.TableRepository<TbWalletChargingRequest>().CreateAsync(chargingRequest, Guid.Parse(userId));

                await _unitOfWork.CommitAsync();
                return proccessPayment;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
            }

            return new PaymentResultDto
            {
                Success = false,
                Message = "Failed to initiate charging request."
            };
        }

        public async Task<bool> ProcessChargingAsync(string userId, decimal amount, Guid chargingRequestId)
        {
            _logger.Information("Starting Charge Process for User: {UserId}, Amount: {Amount}, RequestId: {RequestId}", userId, amount, chargingRequestId);

            if (amount <= 0)
            {
                _logger.Warning("Charge failed: Amount must be positive. User: {UserId}", userId);
                throw new ArgumentException("Amount must be positive");
            }

            // Update Charging Request Status
            var chargingRequestRepo = _unitOfWork.TableRepository<TbWalletChargingRequest>();
            var chargingRequest = await chargingRequestRepo.FindByIdAsync(chargingRequestId);

            if (chargingRequest == null)
            {
                _logger.Warning("Charge failed: Charging Request not found. RequestId: {RequestId}", chargingRequestId);
                return false;
            }

            if (chargingRequest.Status == WalletTransactionStatus.Completed)
            {
                _logger.Information("Charge skipped: Request already processed (Accepted). RequestId: {RequestId}", chargingRequestId);
                return true;
            }

            chargingRequest.Status = WalletTransactionStatus.Completed;
            await chargingRequestRepo.UpdateAsync(chargingRequest, Guid.Parse(userId));

            var result = await ProcessTransactionAsync(userId, amount, WalletTransactionType.Deposit, WalletTransactionDirection.In, chargingRequestId, "Charging");

            if (result) _logger.Information("Charge completed successfully. request: {RequestId}", chargingRequestId);
            else _logger.Error("Charge failed during internal transaction processing. RequestId: {RequestId}", chargingRequestId);

            return result;
        }

        public async Task<bool> VerifyChargingPaymentAsync(string gatewayTransactionId, bool isSuccess, string? failureReason)
        {
            _logger.Information("Verifying Charging Payment. GatewayTxId: {GatewayTxId}, Success: {Success}", gatewayTransactionId, isSuccess);

            var repo = _unitOfWork.TableRepository<TbWalletChargingRequest>();
            // Assuming GatewayTransactionId is unique and indexed
            // We need to find the request. 
            // BEWARE: This might need a specific query method depending on repo capabilities
            var allRequests = await repo.GetAllAsync();
            var chargingRequest = allRequests.FirstOrDefault(r => r.GatewayTransactionId == gatewayTransactionId);

            if (chargingRequest == null)
            {
                _logger.Error("Verify Charging failed: Request not found for GatewayTxId: {GatewayTxId}", gatewayTransactionId);
                return false;
            }

            if (chargingRequest.Status != WalletTransactionStatus.Pending)
            {
                _logger.Information("Verify Charging: Generic Request already processed. Current Status: {Status}", chargingRequest.Status);
                return chargingRequest.Status == WalletTransactionStatus.Completed;
            }

            if (isSuccess)
            {
                return await ProcessChargingAsync(chargingRequest.UserId, chargingRequest.Amount, chargingRequest.Id);
            }
            else
            {
                chargingRequest.Status = WalletTransactionStatus.Failed;
                chargingRequest.FailureReason = failureReason ?? "Payment Failed at Gateway";
                await repo.UpdateAsync(chargingRequest, Guid.Parse(chargingRequest.UserId));
                await _unitOfWork.CommitAsync();
                _logger.Information("Charging Request Rejected/Failed. Id: {Id}, Reason: {Reason}", chargingRequest.Id, failureReason);
                return false;
            }
        }

        #region Helpers Methods

        private async Task<bool> ProcessTransactionAsync(string userId, decimal amount, WalletTransactionType type, WalletTransactionDirection direction, Guid referenceId, string referenceType)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var userGuid = Guid.Parse(userId);
                _logger.Information($"Processing Transaction: User: {userId}, Type: {type}, Direction: {direction}, Amount: {amount}, Ref: {referenceId}");

                var wallet = await GetOrCreateWalletAsync(userId);
                var oldBalance = wallet.Balance;

                // Update Balance
                if (direction == WalletTransactionDirection.In)
                {
                    wallet.Balance += amount;
                }
                else
                {
                    wallet.Balance -= amount;
                }

                wallet.LastTransactionDate = DateTime.UtcNow;

                // Create Transaction Record
                var transaction = new TbCustomerWalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = amount,
                    TransactionType = type,
                    Direction = direction,
                    TransactionStatus = WalletTransactionStatus.Completed,
                    ReferenceId = referenceId,
                    ReferenceType = referenceType
                };

                await _unitOfWork.TableRepository<TbCustomerWallet>().UpdateAsync(wallet, userGuid);
                await _unitOfWork.TableRepository<TbCustomerWalletTransaction>().CreateAsync(transaction, userGuid);

                await _unitOfWork.CommitAsync();

                _logger.Information("Transaction Committed. User: {UserId}, OldBalance: {OldBalance}, NewBalance: {NewBalance}", userId, oldBalance, wallet.Balance);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.Error(ex, "Transaction Failed and Rolled Back. User: {UserId}, Ref: {RefId}", userId, referenceId);
                return false;
            }
        }

        private async Task<TbCustomerWallet> GetOrCreateWalletAsync(string userId)
        {
            var repo = _unitOfWork.TableRepository<TbCustomerWallet>();
            var wallet = await repo.FindAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                wallet = new TbCustomerWallet
                {
                    UserId = userId,
                    Balance = 0,
                    LockedBalance = 0,
                    PendingBalance = 0,
                    LastTransactionDate = null
                };

                // CreateAsync with userId
                await repo.CreateAsync(wallet, Guid.Parse(userId));
                await _unitOfWork.CommitAsync();
            }
            return wallet;
        }
        #endregion
    }
}
