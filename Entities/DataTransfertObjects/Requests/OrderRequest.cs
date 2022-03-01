using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class OrderRequest
    {
        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public string Status { get; set; }
        public PlaceRequest[] Places { get; set; }
    }
}
