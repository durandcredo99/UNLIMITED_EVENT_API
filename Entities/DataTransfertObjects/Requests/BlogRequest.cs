using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BlogRequest
    {
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public Guid CategoryBlogId { get; set; }

        [Required]
        public Guid CommentId { get; set; }

        //[Required]
        //public String AppUserId { get; set; }

       

    }
}
