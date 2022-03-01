using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class BlogParameters : QueryStringParameters
    {
        public BlogParameters()
        {
            OrderBy = "title";
        }
        public Guid? OfCategoryBlogId { get; set; }
        public string OrganizedBy { get; set; }
    }
}
