using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PromoteEventResponse
    {
        public Guid Id { get; set; }
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Date Début")]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Durée en jours")]
        public int duration { get; set; }
        [Display(Name = "Evènement")]
        public Guid EventId { get; set; }
        public EventResponse Event { get; set; }
        [Display(Name = "Position")]
        public Guid PromoteId { get; set; }
        public PromoteResponse Promote { get; set; }
    }
}
