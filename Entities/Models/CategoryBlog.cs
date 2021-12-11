using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class CategoryBlog : IEntity
    {
        public CategoryBlog()
        {
            Blogs = new HashSet<Blog>();
        }
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

    }
}
