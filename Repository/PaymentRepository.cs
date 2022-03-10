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
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private ISortHelper<Payment> _sortHelper;

        public PaymentRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Payment> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Payment>> GetPaymentsAsync(PaymentParameters paymentParameters)
        {
            var payments = Enumerable.Empty<Payment>().AsQueryable();

            ApplyFilters(ref payments, paymentParameters);

            PerformSearch(ref payments, paymentParameters.SearchTerm);

            var sortedPayments = _sortHelper.ApplySort(payments, paymentParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Payment>.ToPagedList
                (
                    sortedPayments,
                    paymentParameters.PageNumber,
                    paymentParameters.PageSize)
                );
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await FindByCondition(payment => payment.Id.Equals(id))
                .Include(x=>x.AppUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaymentExistAsync(Payment payment)
        {
            return false;
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await CreateAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await UpdateAsync(payment);
        }

        public async Task UpdatePaymentAsync(IEnumerable<Payment> payments)
        {
            await UpdateAsync(payments);
        }

        public async Task DeletePaymentAsync(Payment payment)
        {
            await DeleteAsync(payment);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Payment> payments, PaymentParameters paymentParameters)
        {
            payments = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(paymentParameters.AppUserId))
            {
                payments = payments.Where(x => x.AppUserId == paymentParameters.AppUserId);
            }

            if (paymentParameters.MinBirthday != null)
            {
                payments = payments.Where(x => x.Birthday >= paymentParameters.MinBirthday);
            }

            if (paymentParameters.MaxBirthday != null)
            {
                payments = payments.Where(x => x.Birthday < paymentParameters.MaxBirthday);
            }

            if (paymentParameters.MinCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt >= paymentParameters.MinCreateAt);
            }

            if (paymentParameters.MaxCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt < paymentParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Payment> payments, string searchTerm)
        {
            if (!payments.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //payments = payments.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
