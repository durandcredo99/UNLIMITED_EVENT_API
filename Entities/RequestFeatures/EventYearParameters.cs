using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class EventYearParameters : QueryStringParameters
    {
        public EventYearParameters()
        {
            OrderBy = "StartingDate";
        }

    }
}
