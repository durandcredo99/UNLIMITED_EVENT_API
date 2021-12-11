using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PaymentType : IEntity
    {
        public PaymentType()
        {
            Payments = new HashSet<Payment>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
