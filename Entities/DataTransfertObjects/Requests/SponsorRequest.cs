using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SponsorRequest
    {
        public Guid? Id { get; set; }
        public IFormFile File { get; set; }
        public string ImgLink { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Statut du sponsor")]
        public string IsPublic { get; set; }
        [Display(Name = "Numéro de téléphone")]
        public string PhoneSponsor { get; set; }

        [JsonIgnore]
        public String AppUserId { get; set; }
    }
}
