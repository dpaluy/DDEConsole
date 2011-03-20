using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Oracle
{
    public enum SideTypes { BUY, SELL };

    class ExchangeOption : Option
    {
        #region Property
        public decimal Premium { get; set; }
        public int Quantity { get; set; }
        public bool IsBuy
        {
            get
            {
                return (Quantity >= 0);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_strike"></param>
        /// <param name="_exp"></param>
        /// <param name="_vol"></param>
        /// <param name="_premium">Price per 1 option</param>
        /// <param name="_quantity"></param>
        public ExchangeOption(OptionTypes _type, ushort _strike, DateTime _exp, double _vol, decimal _premium, int _quantity)
            :base(_type, _strike, _exp, _vol)
        {
            Premium = _premium;
            Quantity = _quantity;
        }

        public ExchangeOption(Option o, decimal _premium, int _quantity)
            :this(o.OpType, o.StrikePrice, o.Expiration, o.Volatility, _premium, _quantity){}
        #endregion

        #region Value Calculation
        public decimal ValueOnExpirationAtStockPrice()
        {
            decimal result = 0m;
            if (IsBuy)
            {
                if (IsCall)
                    result = (CurrentStockPrice > StrikePrice) ? (PremiaOnExpiration - PremiaOnPurchase) : -PremiaOnPurchase;
                else
                    result = (StrikePrice > CurrentStockPrice) ? (PremiaOnExpiration - PremiaOnPurchase) : -PremiaOnPurchase;
            }
            else // SELL
            {
                if (IsCall)
                    result = (CurrentStockPrice > StrikePrice) ? (-PremiaOnExpiration - PremiaOnPurchase) : -PremiaOnPurchase;
                else
                    result = (StrikePrice > CurrentStockPrice) ? (-PremiaOnExpiration - PremiaOnPurchase) : -PremiaOnPurchase;
            }

            return result;
        }

        private decimal PremiaOnExpiration
        {
            get
            {
                return Math.Abs(Quantity) * 100 * Math.Abs((short)(this.StrikePrice - CurrentStockPrice));
            }
        }

        private decimal PremiaOnPurchase
        {
            get
            {
                return Quantity * Premium;
            }
        }
        #endregion

        #region Output
        public override String ToString()
        {
            // C1250MAY10 1 1$
            String result;
            result = OpType.ToString().Substring(0, 1) + StrikePrice.ToString() + Expiration.ToString("MMMyy", CultureInfo.InvariantCulture) + "  " + Quantity.ToString() + "  " + Premium.ToString() + "$";
            return result;
        }
        #endregion
    }
}
