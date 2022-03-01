using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class SubCategoryParameters : QueryStringParameters
    {
        public SubCategoryParameters()
        {
            OrderBy = "name";
        }
        public string ManagedByAppUserId { get; set; }
    }
}
