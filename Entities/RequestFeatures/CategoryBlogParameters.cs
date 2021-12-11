using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class CategoryBlogParameters : QueryStringParameters
    {
        public CategoryBlogParameters()
        {
            OrderBy = "name";
        }

    }
}
