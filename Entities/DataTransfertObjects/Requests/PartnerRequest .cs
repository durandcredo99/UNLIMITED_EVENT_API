using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PartnerRequest
    {
        public Guid? Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
