using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AppUserResponse
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string NameOrganization { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
        public WorkstationResponse Workstation { get; set; }
    }
}
