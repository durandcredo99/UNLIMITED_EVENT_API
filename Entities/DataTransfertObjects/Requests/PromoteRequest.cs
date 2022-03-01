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
        public int Position { get; set; }
        [Required]
        [Display(Name = "Durée en jours")]
        public int Duration { get; set; }
        [Required]
        public long Amount { get; set; }

        //[Required]
        //[Display(Name = "Nom")]
      
    }
}
