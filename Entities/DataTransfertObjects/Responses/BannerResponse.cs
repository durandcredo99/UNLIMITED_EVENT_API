using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BannerResponse
    {
        public Guid Id { get; set; }

        [Display(Name = "Numéro d'ordre")]
        [Range(1, Int16.MaxValue, ErrorMessage = "Le champ {0} doit être supérieur à {1}.")]
        public int No { get; set; }

        [Display(Name = "URL Bannière")]
        public string ImgLink { get; set; }

        public string Surtitre { get; set; }

        public string Titre { get; set; }
    }
}
