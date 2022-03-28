using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AnnualRateRequest
    {
        public Guid? Id { get; set; }
        [Required]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Commence le")]
        public DateTime StartingDate { get; set; } = DateTime.Now;
        [Required]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Fini le")]
        public DateTime EndingDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Taux de prélèvement")]
        public long Percentage { get; set; }

    }
}
