using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class BlogParameters : QueryStringParameters
    {
        public BlogParameters()
        {
            //OrderBy = "name";
            OrderBy = "title";
        }

    }
}
