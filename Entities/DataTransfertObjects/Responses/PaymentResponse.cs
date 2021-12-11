using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float AmountPaid { get; set; }
        public float AmountRest { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid PaymentTypeId { get; set; }

        public PaymentTypeResponse PaymentType { get; set; }

        public Guid CommandId { get; set; }
        public CommandResponse Command { get; set; }

    }
}
