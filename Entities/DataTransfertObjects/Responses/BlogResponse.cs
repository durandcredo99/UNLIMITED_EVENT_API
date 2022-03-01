using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class BlogResponse
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Display(Name = "Titre")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Guid CategoryBlogId { get; set; }
        public CategoryBlogResponse CategoryBlog { get; set; }
       
        public String AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }
    }
}
