using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class CommentParameters : QueryStringParameters
    {
        public CommentParameters()
        {
            //OrderBy = "name";
            OrderBy = "title";
        }

    }
}
