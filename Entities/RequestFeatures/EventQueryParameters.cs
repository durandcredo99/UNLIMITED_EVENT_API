using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class EventQueryParameters : QueryStringParameters
    {
        public EventQueryParameters()
        {
            OrderBy = "name";
        }

        public string OrganizedBy { get; set; }
        public Guid? OfCategoryId { get; set; }
        public bool PublicOnly { get; set; }

    }
}
