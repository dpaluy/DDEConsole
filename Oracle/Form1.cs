using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oracle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OptionTest();
        }

        void OptionTest()
        {
            DateTime exp = new DateTime(2011, 3, 19);
            decimal cost = 1m;
            Option o1 = new Option(OptionTypes.PUT, 120, exp, cost );
            label1.Text = o1.ToString();
        }
    }
}
