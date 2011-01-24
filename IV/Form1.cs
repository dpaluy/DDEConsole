using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace IV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGraph(zgCall1);
            SetSize();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zgCall1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zgCall1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 20);
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Call IV";
            myPane.XAxis.Title.Text = "Strike";
            myPane.YAxis.Title.Text = "IV";

            // Make up some data arrays based on the Sine function
            double x, y;
            PointPairList list = new PointPairList();
            for (int i = 0; i < 37; i++)
            {
                x = ((double)i - 18.0) / 5.0;
                y = x * x + 1.0;
                list.Add(x, y);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("CALL",
               list, Color.Green, SymbolType.Diamond);

            // Set the Y axis intersect the X axis at an X value of 0.0
            myPane.YAxis.Cross = 0.0;
            // Turn off the axis frame and all the opposite side tics
            myPane.Chart.Border.IsVisible = false;
            myPane.XAxis.MajorTic.IsOpposite = false;
            myPane.XAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;

            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
        }
    }
}
