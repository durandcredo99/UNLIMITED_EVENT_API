using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PromoteEventParameters : QueryStringParameters
    {
        public PromoteEventParameters()
        {
            OrderBy = "StartingDate";
        }
        public string AddFor { get; set; }
    }
}
