using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeightedRate
{
    class Contract
    {
        double Price { get; set; }
        ushort Size { get; set; }

        double Payment
        {
            get
            {
                return Price * Size;
            }
        }

        double Ratio(double weightedRate)
        {
            return Payment / weightedRate;
        }
    }
}
