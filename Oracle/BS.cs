using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oracle
{
    // Calculate Black Sholes
    class BS
    {
        #region Const Units
        const double VEGA_UNIT = 0.01;
        const double THETA_UNIT = (-1 / 365.0);
        const double GAMMA_UNIT = 0.1;
        #endregion

        #region Constructor
        /// <summary>
        /// Black Sholes Constructor
        /// </summary>
        /// <param name="_stockPrice">Current Stock Price</param>
        /// <param name="date_">Current Date</param>
        /// <param name="rate_">Risk Free Rate</param>
        public BS(double stockPrice_, DateTime date_, double rate_)
        {
            CurrentStockPrice = stockPrice_;
            CurrentDate = date_;
            RiskFreeRate = rate_;
        }
        #endregion

        #region Current Data
        public Option option { private get; set; }
        public double CurrentStockPrice { get; set; }
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
        public double RiskFreeRate { get; set; }

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
                return (option.OpType == OptionTypes.CALL) ? NormDist1 : (NormDist1 - 1);
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
                double result = GAMMA_UNIT * NormDist1tag / (CurrentStockPrice * option.Volatility * Math.Sqrt(T_Expiration));
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

                double result = THETA_UNIT * (CurrentStockPrice * option.Volatility * NormDist1tag / (2 * Math.Sqrt(T_Expiration)) + option.StrikePrice * RiskFreeRate * NormDist2 * Math.Exp(-RiskFreeRate * (T_Expiration)));
                if (option.OpType == OptionTypes.PUT)
                {
                    result -= THETA_UNIT * option.StrikePrice * RiskFreeRate * Math.Exp(-RiskFreeRate * (T_Expiration));
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
                double result = option.Volatility * Math.Sqrt(T);
                return result;
            }
        }

        private double Arg1
        {
            get
            {
                double a = Math.Log(CurrentStockPrice / Convert.ToDouble(option.StrikePrice)) + T * (RiskFreeRate + option.Volatility * option.Volatility * 0.5);

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
                return Math.Exp(-Arg1 * Arg1 * 0.5) / Math.Sqrt(2 * Math.PI);
            }
        }
        #endregion

    }
}
