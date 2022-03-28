using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class AnnualRateParameters : QueryStringParameters
    {
        public AnnualRateParameters()
        {
            OrderBy = "StartingDate";
        }

    }
}
