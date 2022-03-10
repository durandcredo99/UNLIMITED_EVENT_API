using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PromoteEvent : IEntity
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        [Required]
        public int duration { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Required]
        public Guid PromoteId { get; set; }

        [ForeignKey("PromoteId")]
        public Promote Promote { get; set; }

        //[Required]
        //public string AppUserId { get; set; }
    }
}
