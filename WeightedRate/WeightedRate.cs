using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WeightedRate
{
    class WeightedRate
    {
        public string Asset { get; set; }
        public string Strike { get; set; }
        public List<Contract> contracts;

        public WeightedRate(string _asset, string _strike)
        {
            Asset = _asset;
            Strike = _strike;
        }

        public void AddContract(Contract c)
        {
            
        }

    }
}
