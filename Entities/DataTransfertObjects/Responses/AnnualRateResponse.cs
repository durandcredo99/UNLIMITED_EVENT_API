using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class AnnualRateResponse
    {
        public Guid Id { get; set; }
        [Display(Name = "Date Début")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Date Fin")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime EndingDate { get; set; }
        [Display(Name = "Pourcentage")]
        public long Percentage { get; set; }
    }
}
