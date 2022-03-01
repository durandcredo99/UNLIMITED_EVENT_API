using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SubCategory : IEntity
    {
        public SubCategory()
        {
            Commercials = new HashSet<Commercial>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Commercial> Commercials { get; set; }
    }
}
