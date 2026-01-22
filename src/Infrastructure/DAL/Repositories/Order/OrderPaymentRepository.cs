using BL.Contracts.GeneralService;
using Common.Enumerations.Payment;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order;

/// <summary>
/// FINAL OrderPaymentRepository - Optimized with Includes
/// ✅ All methods with proper includes for performance
/// ✅ No N+1 query problems
/// </summary>
public class OrderPaymentRepository : TableRepository<TbOrderPayment>, IOrderPaymentRepository
{
    public OrderPaymentRepository(
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        ILogger logger)
        : base(dbContext, currentUserService, logger)
    {
    }

    /// <summary>
    /// Get single order payment (latest) without details
    /// </summary>
    public async Task<TbOrderPayment?> GetOrderPaymentAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDateUtc)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// ✅ NEW: Get single order payment WITH details (Order + PaymentMethod)
    /// Optimized for PaymentService.GetPaymentStatusAsync
    /// </summary>
    public async Task<TbOrderPayment?> GetOrderPaymentWithDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .Include(p => p.Order)
                .Include(p => p.PaymentMethod)
                .OrderByDescending(p => p.CreatedDateUtc)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment with details for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get all order payments without details
    /// </summary>
    public async Task<List<TbOrderPayment>> GetOrderPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .Include(p => p.Order)
                .Include(p => p.PaymentMethod)
                .OrderByDescending(p => p.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payments for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// ✅ NEW: Get payment by ID WITH details
    /// Optimized for PaymentService.GetPaymentByIdAsync
    /// </summary>
    public async Task<TbOrderPayment?> GetPaymentWithDetailsAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.Id == paymentId && !p.IsDeleted)
                .Include(p => p.Order)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment with details {PaymentId}", paymentId);
            throw;
        }
    }

    /// <summary>
    /// Get payment by gateway transaction ID
    /// </summary>
    public async Task<TbOrderPayment?> GetByGatewayTransactionIdAsync(
        string gatewayTransactionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.GatewayTransactionId == gatewayTransactionId && !p.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment by gateway transaction ID {TransactionId}",
                gatewayTransactionId);
            throw;
        }
    }

    /// <summary>
    /// ✅ NEW: Get payment by transaction ID WITH details
    /// Optimized for PaymentService.VerifyPaymentAsync
    /// </summary>
    public async Task<TbOrderPayment?> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.TransactionId == transactionId && !p.IsDeleted)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment by transaction ID {TransactionId}", transactionId);
            throw;
        }
    }

    /// <summary>
    /// Get payment by ID without details
    /// </summary>
    public async Task<TbOrderPayment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.Id == paymentId && !p.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payment {PaymentId}", paymentId);
            throw;
        }
    }

    /// <summary>
    /// Create payment record
    /// </summary>
    public async Task<TbOrderPayment> CreateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default)
    {
        try
        {
            payment.CreatedDateUtc = DateTime.UtcNow;
            payment.IsDeleted = false;

            await _dbContext.Set<TbOrderPayment>().AddAsync(payment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return payment;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating payment for order {OrderId}", payment.OrderId);
            throw;
        }
    }

    /// <summary>
    /// Update payment record
    /// </summary>
    public async Task UpdateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default)
    {
        try
        {
            payment.UpdatedDateUtc = DateTime.UtcNow;

            _dbContext.Set<TbOrderPayment>().Update(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating payment {PaymentId}", payment.Id);
            throw;
        }
    }

    /// <summary>
    /// Get pending payments for order
    /// </summary>
    public async Task<List<TbOrderPayment>> GetPendingPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId
                    && p.PaymentStatus == PaymentStatus.Pending
                    && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting pending payments for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get completed payments for order
    /// Used for payment summary calculations
    /// </summary>
    public async Task<List<TbOrderPayment>> GetCompletedPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId
                    && p.PaymentStatus == PaymentStatus.Completed
                    && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting completed payments for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get payments by payment method type
    /// Useful for filtering wallet/card payments
    /// </summary>
    public async Task<List<TbOrderPayment>> GetPaymentsByMethodTypeAsync(
        Guid orderId,
        PaymentMethodType methodType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId
                    && p.PaymentMethodType == methodType
                    && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payments by method type for order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get failed payments for order
    /// Useful for retry logic
    /// </summary>
    public async Task<List<TbOrderPayment>> GetFailedPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId
                    && p.PaymentStatus == PaymentStatus.Failed
                    && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting failed payments for order {OrderId}", orderId);
            throw;
        }
    }
}