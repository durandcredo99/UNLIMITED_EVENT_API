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
        [Required]
        public string AppUserId { get; set; }
        public float MoneyAmount { get; set; }
        public float RemainingAmount { get; set; }
        public DateTime? PaidAt { get; set; }

        public string Feda_Klass { get; set; }
        public string Feda_Id { get; set; }
        public string Feda_Amount { get; set; }
        public string Feda_Description { get; set; }
        public string Feda_CallbackUrl { get; set; }
        public string Feda_Status { get; set; }
        public string Feda_Customer_id { get; set; }
        public string Feda_Currency_id { get; set; }
        public string Feda_Mode { get; set; }
        
        public Guid? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public Guid? PromoteId { get; set; }

        [ForeignKey("PromoteId")]
        public virtual Promote Promote { get; set; }

        public Guid? PromoteEventId { get; set; }

        [ForeignKey("PromoteEventId")]
        public virtual PromoteEvent PromoteEvent { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
