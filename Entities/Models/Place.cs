using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Place : IEntity
    {

        public Guid Id { get; set; }
        public int NoPlace { get; set; }
        public float Price { get; set; }
        [Required]
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Required]
        public Guid CommandId { get; set; }   

        [ForeignKey("CommandId")]
        public Command Command { get; set; }

    }
}
