using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class EventRequest
    {
        public Guid? Id { get; set; }
        public IFormFile EventImg { get; set; }
        public string ImgLink { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [BindProperty, DataType(DataType.DateTime)]
        //public DateTime Date { get; set; } = DateTime.Now;
        public DateTime Date { get; set; } = DateTime.Today;
        [Required]
        [Display(Name = "Prix")]
        public long Price { get; set; }
        [Required]
        [Display(Name = "Statut de l'événement")]
        public string IsPublic { get; set; }
        [Display(Name = "Statut")]
        public string StatusEvent { get; set; }
        [Required]
        [Display(Name = "Places")]
        public int NbrPlace { get; set; }
        public int NoPriority { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        //[JsonIgnore]
        [Required]
        public string AppUserId { get; set; }

        public Guid SponsorId { get; set; }
        public IFormFile File { get; set; }

        public string Lieu { get; set; }
       
    }
}
