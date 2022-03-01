using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BlogRequest
    {
        public Guid? Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        [Display(Name = "Titre")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public Guid CategoryBlogId { get; set; }

        [Required]
        public Guid CommentId { get; set; }
        public IFormFile File { get; set; }
        //[Required]
        //public String AppUserId { get; set; } [JsonIgnore]
        [JsonIgnore]
        public String AppUserId { get; set; }



    }
}
