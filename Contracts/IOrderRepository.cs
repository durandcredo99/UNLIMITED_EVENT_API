
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<PagedList<Order>> GetOrdersAsync(OrderParameters orderParameters);

        Task<Order> GetOrderByIdAsync(Guid id);
        Task<bool> OrderExistAsync(Order order);

        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task UpdateOrderAsync(IEnumerable<Order> orders);
        Task DeleteOrderAsync(Order order);
    }
}
