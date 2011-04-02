using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NDde.Client;

namespace WeightedRate
{
    public partial class Form1 : Form
    {
        #region Properties
        private System.IO.StreamWriter log;
        private enum TOPICS { VOLUME = 0, LAST, LAST_SIZE};
        private DdeClient[] clients;
        #endregion

        #region Form
        public Form1()
        {
            InitializeComponent();
            OpenLogFile();
            clients = new DdeClient[(int)(TOPICS.VOLUME) + 1];
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Close();
        }
        #endregion

        #region Log
        private void OpenLogFile()
        {
            if (!System.IO.File.Exists("logfile.txt"))
            {
                log = new System.IO.StreamWriter("logfile.txt");
            }
            else
            {
                log = System.IO.File.AppendText("logfile.txt");
            }
            log.WriteLine("----------------------------------------------------------------------------------");
            log.WriteLine("Start Logging " + getTimeStamp());
        }

        private string getTimeStamp()
        {
            return (DateTime.Now.ToString());
        }
        #endregion

        #region Volume
        private void CollectVolume()
        {
            string _topic = "VOLUME";
            string _myapp = Properties.Settings.Default.AppDDE;
            string expiration = "110416";
            string item = "'.SPY" + expiration;  //'.SPY110416C132'
            DdeClient client;

            try
            {
                // Create a client that connects to 'myapp|topic'. 
                using (client = new DdeClient(_myapp, _topic))
                {
                    // Subscribe to the Disconnected event.  This event will notify the application when a conversation has been terminated.
                    client.Disconnected += OnDisconnectedVolume;

                    // Connect to the server.  It must be running or an exception will be thrown.
                    client.Connect();

                    // Advise Loop
                    for (int j = 0; j < 2; j++)
                    {
                        string cp = (j == 0) ? "C" : "P";
                        int strike = 127;
                        for (int i = 0; i < 10; i++)
                        {
                            ++strike;
                            string _item = item + cp + strike.ToString();
                            client.StartAdvise(_item, 1, true, 60000);
                        }
                    }
                    client.Advise += OnAdviseVolume;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                log.WriteLine(getTimeStamp() + " VOLUME " + e.ToString());
            }
        }

        private void OnAdviseVolume(object sender, DdeAdviseEventArgs args)
        {
            log.WriteLine("Volume: " + args.Item + " " + args.Text + " " + getTimeStamp());
        }

        private void OnDisconnectedVolume(object sender, DdeDisconnectedEventArgs args)
        {
            log.WriteLine(getTimeStamp() + " " +
                "OnDisconnected: " +
                "IsServerInitiated=" + args.IsServerInitiated.ToString() + " " +
                "IsDisposed=" + args.IsDisposed.ToString());
        }
        #endregion

        #region Size
        private void CollectSize()
        {
            string _topic = "LAST_SIZE";
            string _myapp = Properties.Settings.Default.AppDDE;
            string expiration = "110416";
            string item = "'.SPY" + expiration;  //'.SPY110416C132'
            DdeClient client;

            try
            {
                // Create a client that connects to 'myapp|topic'. 
                using (client = new DdeClient(_myapp, _topic))
                {
                    // Subscribe to the Disconnected event.  This event will notify the application when a conversation has been terminated.
                    client.Disconnected += OnDisconnectedSize;

                    // Connect to the server.  It must be running or an exception will be thrown.
                    client.Connect();

                    // Advise Loop
                    for (int j = 0; j < 2; j++)
                    {
                        string cp = (j == 0) ? "C" : "P";
                        int strike = 127;
                        for (int i = 0; i < 10; i++)
                        {
                            ++strike;
                            string _item = item + cp + strike.ToString();
                            client.StartAdvise(_item, 1, true, 60000);
                        }
                    }
                    client.Advise += OnAdviseSize;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                log.WriteLine(getTimeStamp() + " Size " + e.ToString());
            }
        }

        private void OnAdviseSize(object sender, DdeAdviseEventArgs args)
        {
            log.WriteLine("Size: " + args.Item + " " + args.Text + " " + getTimeStamp());
        }

        private void OnDisconnectedSize(object sender, DdeDisconnectedEventArgs args)
        {
            log.WriteLine(getTimeStamp() + " " +
                "OnDisconnected: " +
                "IsServerInitiated=" + args.IsServerInitiated.ToString() + " " +
                "IsDisposed=" + args.IsDisposed.ToString());
        }
        #endregion

        #region Price
        private void CollectPrice()
        {
            string _topic = "LAST";
            string _myapp = Properties.Settings.Default.AppDDE;
            string expiration = "110416";
            string item = "'.SPY" + expiration;  //'.SPY110416C132'
            DdeClient client;

            try
            {
                // Create a client that connects to 'myapp|topic'. 
                using (client = new DdeClient(_myapp, _topic))
                {
                    // Subscribe to the Disconnected event.  This event will notify the application when a conversation has been terminated.
                    client.Disconnected += OnDisconnectedPrice;

                    // Connect to the server.  It must be running or an exception will be thrown.
                    client.Connect();

                    client.StartAdvise("SPY", 1, true, 60000);
                    // Advise Loop
                    for (int j = 0; j < 2; j++)
                    {
                        string cp = (j == 0) ? "C" : "P";
                        int strike = 127;
                        for (int i = 0; i < 10; i++)
                        {
                            ++strike;
                            string _item = item + cp + strike.ToString();
                            client.StartAdvise(_item, 1, true, 60000);
                        }
                    }
                    client.Advise += OnAdvisePrice;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                log.WriteLine(getTimeStamp() + " Price " + e.ToString());
            }
        }

        private void OnAdvisePrice(object sender, DdeAdviseEventArgs args)
        {
            log.WriteLine("Last: " + args.Item + " " + args.Text + " " + getTimeStamp());
        }

        private void OnDisconnectedPrice(object sender, DdeDisconnectedEventArgs args)
        {
            log.WriteLine(getTimeStamp() + " " +
                "OnDisconnected: " +
                "IsServerInitiated=" + args.IsServerInitiated.ToString() + " " +
                "IsDisposed=" + args.IsDisposed.ToString());
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            CollectVolume();
            CollectSize();
            CollectPrice();
        }
    }
}
