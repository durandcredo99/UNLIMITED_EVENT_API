using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class CommandParameters : QueryStringParameters
    {
        public CommandParameters()
        {
            OrderBy = "Date";
        }

    }
}
