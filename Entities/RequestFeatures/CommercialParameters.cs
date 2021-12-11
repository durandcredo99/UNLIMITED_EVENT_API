using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class CommercialParameters : QueryStringParameters
    {
        public CommercialParameters()
        {
            //OrderBy = "name";
            OrderBy = "title";
        }

    }
}
