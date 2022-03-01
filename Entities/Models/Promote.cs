using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Promote : IEntity
    {
        public Guid Id { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public long Amount { get; set; }
    }
}
