using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SponsorResponse
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "Statut du sponsor")]
        public string IsPublic { get; set; }
        [Display(Name = "Téléphone Sponsor")]
        public string PhoneSponsor { get; set; }
        public string AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }
    }
}
