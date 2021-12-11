using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SponsorRequest
    {
        public Guid? Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public string PhoneSponsor { get; set; }
        public IFormFile File { get; set; }
    }
}
