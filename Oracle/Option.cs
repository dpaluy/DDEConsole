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
        public ushort ReferencePrice { get; set; }
        public decimal Cost { get; set; }
        public OptionTypes OpType { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Refers to the standard deviation of the continuously compounded returns of a financial instrument within a specific time horizon.
        /// It is used to quantify the risk of the financial instrument over the specified time period.
        /// Implied Volatility -- the volatility of the option implied by current market prices.
        /// <b>NOTE</b> Volatility however cannot be directly observed and must be estimated.
        /// </summary>
        public double Volatility { get; set; }
        #endregion

        #region Constructor
        public Option(OptionTypes _type, ushort _strike, DateTime _exp, decimal _cost)
        {
            OpType = _type;
            ReferencePrice = _strike;
            Expiration = _exp;
            Cost = _cost;

            // Set Defaults
            Quantity = 0;
            Volatility = 0.1;
        }
        #endregion

        #region Output
        public override  String ToString()
        {
            // C1250MAY10
            String result;
            result = OpType.ToString().Substring(0, 1) + ReferencePrice.ToString() + Expiration.ToString("MMMyy", CultureInfo.InvariantCulture) + "  " + Quantity.ToString();
            return result;
        }
        #endregion

        #region Current Data
        public decimal CurrentStockPrice{ private get; set; }
        public double Rate { private get; set; }
        public ushort DaysTillExpiration { get; private set; }
        public ushort WorkingDaysTillExpiration { get; private set; }
        public DateTime CurrentDate { get; set; }
        #endregion

        #region Greeks
        /// <summary>
        /// The degree to which an option price will move given a small change in the underlying stock price.
        /// For example, an option with a delta of 0.5 will move half a cent for every full cent movement in the underlying stock. 
        /// </summary>
        public decimal Delta 
        {
            get
            {
                return (OpType == OptionTypes.CALL) ? Statistics.NORMSDIST(Arg1) : (Statistics.NORMSDIST(Arg1) - 1);
            }
        }
        /// <summary>
        /// It measures how fast the delta changes for small changes in the underlying stock price.
        /// ie the delta of the delta. 
        /// </summary>
        public decimal Gamma
        {
            get;
        }
        /// <summary>
        /// The change in option price given a one day decrease in time to expiration. 
        /// </summary>
        public decimal Theta
        {
            get;
        }
        /// <summary>
        /// The change in option price given a one percentage point change in volatility.
        /// </summary>
        public decimal Vega
        {
            // =s*SQRT((t/365))*nd11*vdif
            get
            {
                return ;
            }
        }
        #endregion


        #region Greeks Tools
        private double WorkingDaysRange
        {
            get
            {
                return (WorkingDaysTillExpiration / WorkingDaysRange);
            }
        }
        private double VolatilityWorkingDays
        {
            get
            {
                return Volatility * Math.Sqrt(WorkingDaysRange); ;
            }
        }

		/*
		S= Stock price

X=Strike price

T=Years to maturity

r= Risk-free rate

v=Volatility
		*/
        private double Arg1
        {
		// 250 Working days inn a year
		
            get
            {
                int exercisePrice = Convert.ToInt32(CurrentStockPrice);
                double a = Math.Log(ReferencePrice / exercisePrice) + WorkingDaysRange * (Rate + Volatility * Volatility * 0.5);

                double result = a / VolatilityWorkingDays;
                return result;
            }
        }

        private double Arg2
        {
            get
            {
                return Arg1 - VolatilityWorkingDays;
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
