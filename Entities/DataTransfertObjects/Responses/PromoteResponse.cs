using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PromoteResponse
    {
        public Guid Id { get; set; }
        public int Position { get; set; }
        [Display(Name = "Montant par jour")]
        public long Amount { get; set; }
    }
}
