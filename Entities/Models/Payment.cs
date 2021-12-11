using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Payment : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float AmountPaid { get; set; }
        public float AmountRest { get; set; }
        public DateTime? PaymentDate { get; set; }

        [Required]
        public Guid PaymentTypeId { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }

        [Required]
        public Guid CommandId { get; set; }

        [ForeignKey("CommandId")]
        public Command Command { get; set; }

    }
}
