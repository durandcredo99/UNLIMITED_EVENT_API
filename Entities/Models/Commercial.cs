using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Commercial : IEntity
    {

        public Guid Id { get; set; }
        [Required]
        public string ImgLink { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Lieu { get; set; }
        public int Number { get; set; }

        [Required]
        public Guid SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public SubCategory SubCategory { get; set; }
    }
}