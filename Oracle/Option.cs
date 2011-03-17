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
        public ushort ExercisePrice { get; set; }
        public decimal Cost { get; set; }
        public OptionTypes OpType { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Refers to the standard deviation of the continuously compounded returns of a financial instrument within a specific time horizon.
        /// It is used to quantify the risk of the financial instrument over the specified time period.
        /// Implied Volatility -- the volatility of the option implied by current market prices.
        /// <b>NOTE</b> Volatility however cannot be directly observed and must be estimated.
        /// </summary>
        public decimal Volatility { get; set; }
        #endregion

        #region Constructor
        public Option(OptionTypes _type, ushort _strike, DateTime _exp, decimal _cost)
        {
            OpType = _type;
            ExercisePrice = _strike;
            Expiration = _exp;
            Cost = _cost;

            // Set Defaults
            Quantity = 0;
            Volatility = 0.1m;
        }
        #endregion

        #region Output
        public override  String ToString()
        {
            // C1250MAY10
            String result;
            result = OpType.ToString().Substring(0, 1) +Strike.ToString() + Expiration.ToString("MMMyy", CultureInfo.InvariantCulture) + "  " + Quantity.ToString();
            return result;
        }
        #endregion

        #region Current Data
        public decimal CurrentStockPrice{ private get; set; }
        public decimal Rate { private get; set; }
        public ushort DaysTillExpiration { get; private set; }
        public ushort WorkingDaysTillExpiration { get; private set; }
        public DateTime CurrentDate { get; set; }
        #endregion
/*
        #region Greeks
        /// <summary>
        /// The degree to which an option price will move given a small change in the underlying stock price.
        /// For example, an option with a delta of 0.5 will move half a cent for every full cent movement in the underlying stock. 
        /// </summary>
        public decimal Delta 
        {
            get; 
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
            get;
        }
        #endregion
*/

        #region Greeks Tools
        //=(LN(s/e)+(rate+v*v*0.5)*(C3/250))/(v*SQRT((C3/250)))
        public double Arg1
        {
            get
            {
                int exercisePrice = Convert.ToInt32(CurrentStockPrice);
                double a = Math.Log(CurrentStockPrice / exercisePrice);
                return 0;
            }
        }

        #endregion

    }
}
