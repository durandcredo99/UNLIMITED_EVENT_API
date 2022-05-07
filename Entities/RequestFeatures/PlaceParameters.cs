using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PlaceParameters : QueryStringParameters
    {
        public PlaceParameters()
        {
            OrderBy = "noPlace";
        }
        public Guid? EventId { get; set; }
        public string BookededBy { get; set; }
    }
}
