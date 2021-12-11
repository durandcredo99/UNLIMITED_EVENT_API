using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Blog : IEntity
    {
        public Blog()
        {
            Comments = new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public DateTime Date { get; set; }
       
        [Required]
        public Guid CategoryBlogId { get; set; }

        [ForeignKey("CategoryBlogId")]
        public CategoryBlog CategoryBlog { get; set; }

        [Required]
        public String AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
