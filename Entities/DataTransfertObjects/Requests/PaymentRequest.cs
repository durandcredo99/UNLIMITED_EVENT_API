using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PaymentRequest
    {
        public Guid Id { get; set; }
        //[Required]
        public Guid PromoteId { get; set; }
        public Guid PromoteEventId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Display(Name = "Montant Reçu")]
        [Range(10.0, Double.MaxValue, ErrorMessage = "Le champ {0} doit être supérieur à {1}.")]
        public float MoneyAmount { get; set; }
        [Display(Name = "Reliquat")]
        public float RemainingAmount { get; set; }
        [Display(Name = "Payé le")]
        public DateTime? PaidAt { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        public string Feda_Klass { get; set; }
        public string Feda_Id { get; set; }
        public string Feda_Amount { get; set; }
        public string Feda_Reference { get; set; }
        public string Feda_Description { get; set; }
        public string Feda_CallbackUrl { get; set; }
        public string Feda_Status { get; set; }
        public string Feda_Customer_id { get; set; }
        public string Feda_Currency_id { get; set; }
        public string Feda_Mode { get; set; }
    }
}
