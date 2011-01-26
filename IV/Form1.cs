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

        private PointPairList makePoints()
        {
            PointPairList list = new PointPairList();
            double x, y;
            for (int i = 0; i < 10; i++)
            {
                x = 120 + i;
                y = i + 1.0;
                list.Add(x, y);
            }
            return list;
        }

        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];

            return curve.Label.Text + " IV is " + pt.Y.ToString("f2") + "% " + pt.X.ToString("f1") + " strike";
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Call IV";
            myPane.XAxis.Title.Text = "Strike";
            myPane.YAxis.Title.Text = "IV";

            PointPairList list = makePoints();

            // Generate a red curve with diamond
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

            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = 20;
            myPane.XAxis.Scale.Min = 100;
            myPane.XAxis.Scale.Max = 150;

            // Enable scrollbars if needed
            zgc.IsShowHScrollBar = true;
            zgc.IsShowVScrollBar = true;
            zgc.IsAutoScrollRange = true;
            zgc.IsScrollY2 = true;

            // Show tooltips when the mouse hovers over a point
            zgc.IsShowPointValues = true;
            zgc.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
            // Make sure the Graph gets redrawn
            zgc.Invalidate();
        }
    }
}
