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
        [Display(Name = "Commence le")]
        [BindProperty, DataType(DataType.DateTime)]
        public DateTime StartsOn { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Se termine")]
        [BindProperty, DataType(DataType.DateTime)]
        public DateTime EndsOn { get; set; } = DateTime.Now.AddDays(1);

        [Required]
        [Display(Name = "Statut de l'événement")]
        public string IsPublic { get; set; }

        [Display(Name = "Statut")]
        public string StatusEvent { get; set; }

        public int NoPriority { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        //[JsonIgnore]
        [Required]
        public string AppUserId { get; set; }

        public Guid SponsorId { get; set; }
        
        [Required]
        [Display(Name = "Bannière")]
        public IFormFile File { get; set; }

        public string Lieu { get; set; }
        [Display(Name = "J'utilise mon propre creat")]
        public bool MyOwnCreat { get; set; }
       
    }
}
