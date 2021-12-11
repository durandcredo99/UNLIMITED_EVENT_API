using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.RequestFeatures
{
    public class PaymentParameters : QueryStringParameters
    {
        public PaymentParameters()
        {
            OrderBy = "name";
        }

    }
}
