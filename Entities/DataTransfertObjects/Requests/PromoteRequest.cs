using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PromoteRequest
    {
        public Guid? Id { get; set; }
        [Required]

        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Position { get; set; }

        [Required]
        [Display(Name = "Montant par jour")]
        public long Amount { get; set; }

        //[Required]
        //[Display(Name = "Nom")]
      
    }
}
