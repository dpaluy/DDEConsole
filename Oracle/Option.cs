using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oracle
{
    class Option
    {
        enum OptionTypes {CALL, PUT};

        private DateTime expiration_;
        public DateTime Expiration
        {
            get
            {
                return expiration_;
            }
            set
            {
                expiration_ = value;
            }
        }

        private ushort strike_;
        public ushort Strike
        {
            get
            {
                return strike_;
            }
        }

        private OptionTypes opType_;
        public OptionTypes TheType
        {
            get 
            {
                return opType_;
            }
        }

        public String ToString
        {
            get
            {
                // C1250MAY10
                return opType_.ToString + expiration_.ToString + expiration_.ToShortDateString;
            }
        }
    }
}
