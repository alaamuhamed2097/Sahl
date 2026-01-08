using BL.Contracts.GeneralService;
using Common.Enumerations.Payment;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order;

/// <summary>
/// Repository for order payment operations
/// </summary>
public class OrderPaymentRepository : TableRepository<TbOrderPayment>, IOrderPaymentRepository
{
    public OrderPaymentRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
        : base(dbContext, currentUserService, logger)
    {
    }

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

    public async Task<List<TbOrderPayment>> GetOrderPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrderPayment>()
                .AsNoTracking()
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting payments for order {OrderId}", orderId);
            throw;
        }
    }

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
}