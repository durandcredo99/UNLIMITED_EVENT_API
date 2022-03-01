using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PlaceParameters : QueryStringParameters
    {
        public PlaceParameters()
        {
            OrderBy = "name";
        }
        public Guid? EventId { get; set; }
    }
}
