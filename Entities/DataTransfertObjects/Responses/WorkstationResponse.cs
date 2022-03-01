
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class WorkstationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public IList<ClaimResponse> Claims { get; set; }
        public virtual AppUserResponse[] AppUsers { get; set; }
    }
}
