
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWorkstationRepository
    {
        Task<PagedList<Workstation>> GetWorkstationsAsync(WorkstationParameters workstationParameters);

        Task<Workstation> GetWorkstationByIdAsync(string id);
        Task<Workstation> GetWorkstationByNameAsync(string workstationName);
        Task<IList<Claim>> GetClaimsAsync(Workstation workstation);
        Task<bool> AddClaimsSucceededAsync(Workstation workstation, IEnumerable<Claim> claims);
        Task<bool> RemoveClaimsSucceededAsync(Workstation workstation, IList<Claim> claims);
        Task<bool> WorkstationExistAsync(Workstation workstation);

        Task CreateWorkstationAsync(Workstation workstation);
        Task UpdateWorkstationAsync(Workstation workstation);
        Task DeleteWorkstationAsync(Workstation workstation);
    }
}
