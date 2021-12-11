using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Events = new HashSet<Event>();
        }

        public string ImgLink { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
