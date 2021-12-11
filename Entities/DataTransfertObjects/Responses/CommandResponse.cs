using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CommandResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public String AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }
    }
}
