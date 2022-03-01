using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PromoteEventRequest
    {
        public Guid? Id { get; set; }
        [Required]
        [BindProperty, DataType(DataType.Date)]
        public DateTime StartingDate { get; set; } = DateTime.Now;

        [Required]
        public int duration { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public Guid PromoteId { get; set; }
    }
}
