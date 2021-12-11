using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public bool IsPublic { get; set; }
        public string StatusEvent { get; set; }
        public int NoTicket { get; set; }
        public int NoPriority { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryResponse Category { get; set; }

        public String AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }

        public Guid SponsorId { get; set; }
        public SponsorResponse Sponsor { get; set; }
    }
}
