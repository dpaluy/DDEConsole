using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Oracle
{
    public enum OptionTypes { CALL, PUT };

    class Option
    {
        #region Property
        public DateTime Expiration { get; set; }
        public ushort StrikePrice { get; set; }
        public decimal Cost { get; set; }
        public OptionTypes OpType { get; set; }
        public int Quantity { get; set; }
        #endregion

        #region Constructor
        public Option(OptionTypes _type, ushort _strike, DateTime _exp, decimal _cost, int quantity_)
        {
            OpType = _type;
            StrikePrice = _strike;
            Expiration = _exp;
            Cost = _cost;
            Quantity = quantity_;

            // Set Defaults
            Volatility = 0.1;
        }
        #endregion

        #region Output
        public override  String ToString()
        {
            // C1250MAY10
            String result;
            result = OpType.ToString().Substring(0, 1) + StrikePrice.ToString() + Expiration.ToString("MMMyy", CultureInfo.InvariantCulture) + "  " + Quantity.ToString();
            return result;
        }
        #endregion

        #region Current Data
        public void SetCurrentData(double _stockPrice, DateTime date_, double rate_, double v_)
        {
            CurrentStockPrice = _stockPrice;
            CurrentDate = date_;
            RiskFreeRate = rate_;
            Volatility = v_;
        }
        public double CurrentStockPrice{ private get; set; }
        private DateTime currentDate_;
        public DateTime CurrentDate 
        {
            get
            {
                return currentDate_;
            }
            set
            {
                currentDate_ = value;
                DaysTillExpiration = 170;
                TradeDays = 150;
            }
        }
        public double RiskFreeRate { private get; set; }
        /// <summary>
        /// Refers to the standard deviation of the continuously compounded returns of a financial instrument within a specific time horizon.
        /// It is used to quantify the risk of the financial instrument over the specified time period.
        /// Implied Volatility -- the volatility of the option implied by current market prices.
        /// <b>NOTE</b> Volatility however cannot be directly observed and must be estimated.
        /// </summary>
        public double Volatility { get; set; }

        public ushort DaysTillExpiration { get; private set; }
        public ushort TradeDays { get; private set; }
        
        #endregion

        #region Greeks
        /// <summary>
        /// The degree to which an option price will move given a small change in the underlying stock price.
        /// For example, an option with a delta of 0.5 will move half a cent for every full cent movement in the underlying stock. 
        /// </summary>
        public double Delta 
        {
            get
            {
                return (OpType == OptionTypes.CALL) ? NormDist1 : (NormDist1 - 1);
            }
        }
        /// <summary>
        /// It measures how fast the delta changes for small changes in the underlying stock price.
        /// ie the delta of the delta. 
        /// </summary>
        public double Gamma
        {
            get
            {
                double result = GAMMA_UNIT * NormDist1tag / (CurrentStockPrice * Volatility * Math.Sqrt(T_Expiration));
                return result;
            }
        }
        /// <summary>
        /// The change in option price given a one day decrease in time to expiration. 
        /// </summary>
        public double Theta
        {
            get
            {

                double result = THETA_UNIT * (CurrentStockPrice * Volatility * NormDist1tag / (2 * Math.Sqrt(T_Expiration)) + StrikePrice * RiskFreeRate * NormDist2 * Math.Exp(-RiskFreeRate * (T_Expiration)));                
                if (OpType == OptionTypes.PUT)
                {
                    result -= THETA_UNIT * StrikePrice * RiskFreeRate * Math.Exp(-RiskFreeRate * (T_Expiration));
                }
                return result;
            }
        }
        /// <summary>
        /// The change in option price given a one percentage point change in volatility.
        /// </summary>
        public double Vega
        {
            get
            {
                return CurrentStockPrice * Math.Sqrt(T_Expiration) * NormDist1tag * VEGA_UNIT;
            }
        }
        #endregion

        #region Greeks Tools
        const double VEGA_UNIT = 0.01;
        const double THETA_UNIT = (-1 / 365.0);
        const double GAMMA_UNIT	= 0.1;

        private double T_Expiration
        {
            get
            {
                double result = DaysTillExpiration / 365.0;
                return result;
            }
        }

        /// <summary>
        /// Time to Maturity. (Trade Days / Working Days in a Year). Working Days in a Year = 250
        /// </summary>
        private double T
        {
            get
            {
                double result = TradeDays / 250.0;
                return result;
            }
        }
        private double Volatility_SqrtT
        {
            get
            {
                double result = Volatility * Math.Sqrt(T); 
                return result;
            }
        }

        private double Arg1
        {
            get
            {
                double a = Math.Log( CurrentStockPrice / Convert.ToDouble(StrikePrice)) + T * (RiskFreeRate + Volatility * Volatility * 0.5);

                double result = a / Volatility_SqrtT;
                return result;
            }
        }
        private double Arg2
        {
            get
            {
                return Arg1 - Volatility_SqrtT;
            }
        }
        private double NormDist1
        {
            get
            {
                return Statistics.NORMSDIST(Arg1);
            }
        }
        private double NormDist2
        {
            get
            {
                return Statistics.NORMSDIST(Arg2);
            }
        }

        private double NormDist1tag
        {
            get
            {
                return Math.Exp(-Arg1*Arg1*0.5)/Math.Sqrt(2*Math.PI);
            }
        }
        #endregion

    }
}
