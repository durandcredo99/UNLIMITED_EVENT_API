using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private ISortHelper<Order> _sortHelper;

        public OrderRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Order> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Order>> GetOrdersAsync(OrderParameters orderParameters)
        {
            var orders = Enumerable.Empty<Order>().AsQueryable();

            ApplyFilters(ref orders, orderParameters);

            PerformSearch(ref orders, orderParameters.SearchTerm);

            var sortedOrders = _sortHelper.ApplySort(orders, orderParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Order>.ToPagedList
                (
                    sortedOrders,
                    orderParameters.PageNumber,
                    orderParameters.PageSize)
                );
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await FindByCondition(order => order.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> OrderExistAsync(Order order)
        {
            return false;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await CreateAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await UpdateAsync(order);
        }

        public async Task UpdateOrderAsync(IEnumerable<Order> orders)
        {
            await UpdateAsync(orders);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await DeleteAsync(order);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Order> orders, OrderParameters orderParameters)
        {
            orders = FindAll()
                .Include(x=>x.Places);

            if (!string.IsNullOrWhiteSpace(orderParameters.OfAppUserId))
            {
                orders = orders.Where(x => x.AppUserId == orderParameters.OfAppUserId);
            }

            if (orderParameters.OpenedOnly)
            {
                orders = orders.Where(x => x.Status == STATICS.ORDER_STATUS_OPENED);
            }

            /*
            if (orderParameters.MaxBirthday != null)
            {
                orders = orders.Where(x => x.Birthday < orderParameters.MaxBirthday);
            }

            if (orderParameters.MinCreateAt != null)
            {
                orders = orders.Where(x => x.CreateAt >= orderParameters.MinCreateAt);
            }

            if (orderParameters.MaxCreateAt != null)
            {
                orders = orders.Where(x => x.CreateAt < orderParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Order> orders, string searchTerm)
        {
            if (!orders.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //orders = orders.Where(x => x.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
            orders = orders.Where(x => x.Date.ToString().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
