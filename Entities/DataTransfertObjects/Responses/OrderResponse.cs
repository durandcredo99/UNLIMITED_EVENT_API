using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }
        public PlaceResponse[] Places { get; set; }
    }
}
