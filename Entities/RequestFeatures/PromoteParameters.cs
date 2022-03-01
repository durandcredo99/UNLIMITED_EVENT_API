using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PromoteParameters : QueryStringParameters
    {
        public PromoteParameters()
        {
            OrderBy = "Position";
        }

    }
}
