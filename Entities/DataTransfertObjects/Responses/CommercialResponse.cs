using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CommercialResponse
    {

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Lieu { get; set; }
    }
}
