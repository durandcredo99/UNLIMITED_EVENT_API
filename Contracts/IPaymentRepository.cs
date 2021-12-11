
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentRepository
    {
        Task<PagedList<Payment>> GetPaymentsAsync(PaymentParameters paymentParameters);

        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<bool> PaymentExistAsync(Payment payment);

        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(IEnumerable<Payment> payments);
        Task DeletePaymentAsync(Payment payment);
    }
}
