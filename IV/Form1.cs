using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using NDde.Client;
using System.Reflection;

namespace IV
{
    public partial class Form1 : Form
    {
        private int AssetValue = 100;

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            labelAsset.Text = Properties.Settings.Default.ITEM;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGraph(zgCall1);
            SetSize();
            StartAssetDDE();
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
            for (int i = 0; i < 20; i++)
            {
                x = AssetValue + i;
                y = 1;
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

            // Generate a curve with diamond
            LineItem myCurve = myPane.AddCurve("CALL",
               list, Color.Blue, SymbolType.Diamond);

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
            myPane.XAxis.Scale.Min = AssetValue - 30;
            myPane.XAxis.Scale.Max = AssetValue + 30;

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

        private void StartAssetDDE()
        {
            String _topic = "LAST";

            try
            {
                // Create a client that connects to 'myapp|topic'. 
                using (DdeClient client = new DdeClient(Properties.Settings.Default.APP, _topic))
                {
                    // Subscribe to the Disconnected event.  This event will notify the application when a conversation has been terminated.
                    client.Disconnected += OnDisconnected;

                    // Connect to the server.  It must be running or an exception will be thrown.
                    client.Connect();

                    // Advise Loop
                    client.StartAdvise(Properties.Settings.Default.ITEM, 1, true, 60000);
                    client.Advise += OnAdvise;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Please run " + Properties.Settings.Default.APP + " and restart this application!");
            }
        }

        private void OnAdvise(object sender, DdeAdviseEventArgs args)
        {
            Double tmp = Convert.ToDouble(args.Text.Trim());
            AssetValue = Convert.ToInt32(tmp);
            String value = Convert.ToString(tmp);

            if (this.Visible)
                SetControlPropertyThreadSafe(label1, "Text", value);
        }

        private void OnDisconnected(object sender, DdeDisconnectedEventArgs args)
        {
        }

    }
}
