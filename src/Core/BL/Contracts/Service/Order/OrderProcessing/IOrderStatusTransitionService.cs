using Common.Enumerations.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts.Service.Order.OrderProcessing;

public interface IOrderStatusTransitionService
{
    Task<bool> HandleStatusChangeAsync(Guid orderId, OrderProgressStatus newStatus, string userId);
}
