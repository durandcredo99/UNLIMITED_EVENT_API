using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SponsorResponse
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string PhoneSponsor { get; set; }
    }
}
