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
    public class PaymentTypeRepository : RepositoryBase<PaymentType>, IPaymentTypeRepository
    {
        private ISortHelper<PaymentType> _sortHelper;

        public PaymentTypeRepository(
            RepositoryContext repositoryContext,
            ISortHelper<PaymentType> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<PaymentType>> GetPaymentTypesAsync(PaymentTypeParameters paymentTypeParameters)
        {
            var paymentTypes = Enumerable.Empty<PaymentType>().AsQueryable();

            ApplyFilters(ref paymentTypes, paymentTypeParameters);

            PerformSearch(ref paymentTypes, paymentTypeParameters.SearchTerm);

            var sortedPaymentTypes = _sortHelper.ApplySort(paymentTypes, paymentTypeParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<PaymentType>.ToPagedList
                (
                    sortedPaymentTypes,
                    paymentTypeParameters.PageNumber,
                    paymentTypeParameters.PageSize)
                );
        }

        public async Task<PaymentType> GetPaymentTypeByIdAsync(Guid id)
        {
            return await FindByCondition(paymentType => paymentType.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaymentTypeExistAsync(PaymentType paymentType)
        {
            return await FindByCondition(x => x.Name == paymentType.Name)
                .AnyAsync();
        }

        public async Task CreatePaymentTypeAsync(PaymentType paymentType)
        {
            await CreateAsync(paymentType);
        }

        public async Task UpdatePaymentTypeAsync(PaymentType paymentType)
        {
            await UpdateAsync(paymentType);
        }

        public async Task UpdatePaymentTypeAsync(IEnumerable<PaymentType> paymentTypes)
        {
            await UpdateAsync(paymentTypes);
        }

        public async Task DeletePaymentTypeAsync(PaymentType paymentType)
        {
            await DeleteAsync(paymentType);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<PaymentType> paymentTypes, PaymentTypeParameters paymentTypeParameters)
        {
            paymentTypes = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(paymentTypeParameters.AppUserId))
            {
                paymentTypes = paymentTypes.Where(x => x.AppUserId == paymentTypeParameters.AppUserId);
            }

            if (paymentTypeParameters.MinBirthday != null)
            {
                paymentTypes = paymentTypes.Where(x => x.Birthday >= paymentTypeParameters.MinBirthday);
            }

            if (paymentTypeParameters.MaxBirthday != null)
            {
                paymentTypes = paymentTypes.Where(x => x.Birthday < paymentTypeParameters.MaxBirthday);
            }

            if (paymentTypeParameters.MinCreateAt != null)
            {
                paymentTypes = paymentTypes.Where(x => x.CreateAt >= paymentTypeParameters.MinCreateAt);
            }

            if (paymentTypeParameters.MaxCreateAt != null)
            {
                paymentTypes = paymentTypes.Where(x => x.CreateAt < paymentTypeParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<PaymentType> paymentTypes, string searchTerm)
        {
            if (!paymentTypes.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            paymentTypes = paymentTypes.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
