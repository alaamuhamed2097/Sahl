using AutoMapper;
using BL.Contracts.Service.Order.Payment;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order.Payment;
using Serilog;
using Shared.DTOs.Order.Payment;

namespace BL.Services.Order.Payment
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentMethodService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<PaymentMethodDto>> GetAllPaymentMethodsAsync()
        {
            try
            {
                _logger.Information("Retrieving all payment methods");

                var paymentMethodRepo = _unitOfWork.Repository<TbPaymentMethod>();
                var methods = await paymentMethodRepo.GetAllAsync();

                var result = _mapper.Map<List<PaymentMethodDto>>(methods);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving all payment methods");
                throw;
            }
        }

        public async Task<List<PaymentMethodDto>> GetActivePaymentMethodsAsync()
        {
            try
            {
                _logger.Information("Retrieving active payment methods");

                var paymentMethodRepo = _unitOfWork.Repository<TbPaymentMethod>();
                var methods = await paymentMethodRepo.GetAsync(m => m.IsActive);

                var result = _mapper.Map<List<PaymentMethodDto>>(methods);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving active payment methods");
                throw;
            }
        }

        public async Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(Guid id)
        {
            try
            {
                _logger.Information("Retrieving payment method {PaymentMethodId}", id);

                var paymentMethodRepo = _unitOfWork.Repository<TbPaymentMethod>();
                var method = (await paymentMethodRepo.GetAsync(m => m.Id == id)).FirstOrDefault();

                if (method == null)
                {
                    _logger.Warning("Payment method {PaymentMethodId} not found", id);
                    return null;
                }

                var result = _mapper.Map<PaymentMethodDto>(method);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving payment method {PaymentMethodId}", id);
                throw;
            }
        }
    }
}
