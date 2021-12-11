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
    public class PlaceRequest
    {
        public int NoPlace { get; set; }
        public float Price { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public Guid CommandId { get; set; }

    }
}
