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
            DateTime today = DateTime.Now;
            decimal cost = 1m;
            Option o1 = new Option(OptionTypes.CALL, 31, exp, cost, 1 );
            Option o2 = new Option(OptionTypes.PUT, 31, exp, cost, 1);

            o1.SetCurrentData(32.9, today, 0.025, 0.54);
            o2.SetCurrentData(32.9, today, 0.025, 0.54);

            label1.Text = o1.ToString();
            label10.Text = o2.ToString();

            label2.Text = Convert.ToString(o1.Delta);
            label9.Text = Convert.ToString(o2.Delta);
            label14.Text = "Delta";

            label3.Text = Convert.ToString(o1.Gamma);
            label8.Text = Convert.ToString(o2.Gamma);
            label13.Text = "Gamma";

            label4.Text = Convert.ToString(o1.Vega);
            label7.Text = Convert.ToString(o2.Vega);
            label12.Text = "Vega";

            label5.Text = Convert.ToString(o1.Theta);
            label6.Text = Convert.ToString(o2.Theta);
            label11.Text = "Theta";
        }
    }
}
