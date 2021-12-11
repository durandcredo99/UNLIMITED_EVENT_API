using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class SponsorParameters : QueryStringParameters
    {
        public SponsorParameters()
        {
            OrderBy = "name";
        }

    }
}
