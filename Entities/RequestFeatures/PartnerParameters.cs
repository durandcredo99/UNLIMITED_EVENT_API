using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PartnerParameters : QueryStringParameters
    {
        public PartnerParameters()
        {
            OrderBy = "name";
        }

    }
}
