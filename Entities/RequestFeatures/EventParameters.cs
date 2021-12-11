using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class EventParameters : QueryStringParameters
    {
        public EventParameters()
        {
            OrderBy = "name";
        }

    }
}
