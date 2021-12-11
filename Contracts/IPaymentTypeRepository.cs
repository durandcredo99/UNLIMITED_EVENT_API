
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentTypeRepository
    {
        Task<PagedList<PaymentType>> GetPaymentTypesAsync(PaymentTypeParameters paymentTypeParameters);

        Task<PaymentType> GetPaymentTypeByIdAsync(Guid id);
        Task<bool> PaymentTypeExistAsync(PaymentType paymentType);

        Task CreatePaymentTypeAsync(PaymentType paymentType);
        Task UpdatePaymentTypeAsync(PaymentType paymentType);
        Task UpdatePaymentTypeAsync(IEnumerable<PaymentType> paymentTypes);
        Task DeletePaymentTypeAsync(PaymentType paymentType);
    }
}
