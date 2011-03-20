using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oracle
{
    class Statistics
    {
        // returns the probability that the observed value of a standard normal random variable will be less than or equal to d
        public static double NORMSDIST(double d)
        {
            double erfHolder = Erf(d / Math.Sqrt(2.0));
            return erfHolder + (1 - erfHolder) / 2;
        }
        
        private static double Erf(double n)
        {
            return (2.0 / Math.Sqrt(Math.PI)) * (n - (Math.Pow(n, 3) / 3) + (Math.Pow(n, 5) / 10) -
            (Math.Pow(n, 7) / 42) + (Math.Pow(n, 9) / 216) - (Math.Pow(n, 11) / 1320) + (Math.Pow(n, 13) / 9360) -
            (Math.Pow(n, 15) / 75600) + (Math.Pow(n, 17) / 685440));
        }
    }
}
