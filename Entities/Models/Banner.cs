using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Banner
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        [Required]
        public string ImgLink { get; set; }

        public string Surtitre { get; set; }

        public string Titre { get; set; }
    }
}
