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
        public OrderRequest()
        {
            Places = new List<PlaceRequest>();
        }

        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        [Range(10.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public long Total { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public string Status { get; set; }
        public virtual List<PlaceRequest> Places { get; set; }
    }
}
