using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class CategoryParameters : QueryStringParameters
    {
        public CategoryParameters()
        {
            OrderBy = "name";
        }
        public string ManagedByAppUserId { get; set; }
    }
}
