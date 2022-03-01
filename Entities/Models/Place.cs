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
        public long Price { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
      
        [Required]
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        public Guid? OrderId { get; set; }   

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

    }
}
