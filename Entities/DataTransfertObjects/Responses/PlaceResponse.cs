using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PlaceResponse
    {
        public Guid Id { get; set; }
        [Display(Name = "Numéro de place")]
        public int NoPlace { get; set; }
        [Display(Name = "Prix")]
        public long Price { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Status { get; set; }
        public Guid EventId { get; set; }
        public EventResponse Event { get; set; }
        public Guid? OrderId { get; set; }
        public OrderResponse Order { get; set; }

        public bool IsSelected { get; set; }
    }
}
