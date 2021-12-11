using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Sponsor : IEntity
    {
        public Sponsor()
        {
            Events = new HashSet<Event>();
        }

        public Guid Id { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public string PhoneSponsor { get; set; }

        public virtual ICollection<Event> Events { get; set; }

    }
}
