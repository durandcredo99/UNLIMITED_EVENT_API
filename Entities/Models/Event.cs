using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Event : IEntity
    {
        public Event()
        {
            Places = new HashSet<Place>();
        }

        public Guid Id { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartsOn { get; set; }
        [Required]
        public DateTime EndsOn { get; set; }
        [Required]
        public long Price { get; set; }
        [Required]
        public string IsPublic { get; set; }
        public string StatusEvent { get; set; }
        public string Lieu { get; set; }
        public int NoPriority { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public String AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public Guid SponsorId { get; set; }

        [ForeignKey("SponsorId")]
        public Sponsor Sponsor { get; set; }

        public virtual ICollection<Place> Places { get; set; }

    }
}
