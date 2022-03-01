using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CommercialResponse
    {

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Display(Name = "Titre")]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public string Lieu { get; set; }
        [Display(Name = "Catégorie")]
        public Guid SubCategoryId { get; set; }
        public SubCategoryResponse SubCategory { get; set; }
    }
}
