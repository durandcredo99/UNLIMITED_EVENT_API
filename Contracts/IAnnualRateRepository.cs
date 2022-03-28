
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAnnualRateRepository
    {
        Task<PagedList<AnnualRate>> GetAnnualRatesAsync(AnnualRateParameters annualRateParameters);

        Task<AnnualRate> GetAnnualRateByIdAsync(Guid id);
        Task<bool> AnnualRateExistAsync(AnnualRate annualRate);
        Task<AnnualRate> GetOpenAnnualRateAsync();

        Task CreateAnnualRateAsync(AnnualRate annualRate);
        Task UpdateAnnualRateAsync(AnnualRate annualRate);
        Task UpdateAnnualRateAsync(IEnumerable<AnnualRate> annualRates);
        Task DeleteAnnualRateAsync(AnnualRate annualRate);
    }
}
