using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class OrderParameters : QueryStringParameters
    {
        public OrderParameters()
        {
            OrderBy = "Date";
        }

        public string OfAppUserId { get; set; }
        public bool OpenedOnly { get; set; }
    }
}
