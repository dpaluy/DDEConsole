using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NDde.Client;

namespace CollectData
{     
    class Program
    {
        enum OPTIONS // APRIL
        {
            C1160 = 80701311, P1160 = 80701576,
            C1170 = 80709819, P1170 = 80710064,
            C1180 = 80701303, P1180 = 80701568,
            C1190 = 80709801, P1190 = 80710056,
            
            C1200 = 80701295, P1200 = 80701550,
            C1210 = 80709793, P1210 = 80710049,
            C1220 = 80701287, P1220 = 80701543,
            C1230 = 80709785, P1230 = 80710031,
            C1240 = 80701279, P1240 = 80701535,
            C1250 = 80709777, P1250 = 80710023,
            C1260 = 80701261, P1260 = 80701527,
            C1270 = 80709892, P1270 = 80710148,
            C1280 = 80701253, P1280 = 80701519,
            C1290 = 80709900, P1290 = 80710155,
            
            C1300 = 80701246, P1300 = 80701501,
            C1310 = 80709918, P1310 = 80710163,
            C1320 = 80701238, P1320 = 80701493,
            C1330 = 80709926, P1330 = 80710171,
            C1340 = 80701360, P1340 = 80701626,
            C1350 = 80709934, P1350 = 80710189,
            C1360 = 80701378, P1360 = 80701634,
            C1370 = 80709942, P1370 = 80710197,
            C1380 = 80701386, P1380 = 80701642,
            C1390 = 80709959, P1390 = 80710205,
            C1400 = 80701394, P1400 = 80701659
        };

        enum DDE_VALUES { 
            TSHR, SFUT, SINT, IVVL, OPOS, POSP, AVRP, DTMR, DVOL, BLI1, BL1A, SLI1, SL1A, DLTA};

        private static TextWriter tw;

        static void Main(string[] args)
        {
            // create a writer and open the file
            tw = new StreamWriter("data.txt");

            tw.WriteLine("DDE BIZPORTAL {0:MM/dd/yy}", DateTime.Now);
            string _myapp = "Star32";
            string _topic = "DDE";
       
            try
            {
                // Create a client that connects to 'myapp|topic'. 
                using (DdeClient client = new DdeClient(_myapp, _topic))
                {
                    // Subscribe to the Disconnected event.  This event will notify the application when a conversation has been terminated.
                    client.Disconnected += OnDisconnected;

                    // Connect to the server.  It must be running or an exception will be thrown.
                    client.Connect();

                    // Advise Loop
                    foreach (int val in Enum.GetValues(typeof(OPTIONS)))
                    {
                        foreach(string name in Enum.GetNames(typeof(DDE_VALUES)))
                        {
                            string advice = name + val.ToString();
                            client.StartAdvise(advice, 1, true, 60000);
                        }
                    }
                    
                    client.Advise += OnAdvise;

                    // Wait for the user to press ENTER before proceding.
                    Console.WriteLine("Press ENTER to quit...");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Press ENTER to quit...");
                Console.ReadLine();
            }
            
            // close the stream
            tw.Close();
        }

        private static void OnAdvise(object sender, DdeAdviseEventArgs args)
        {
            string value = Regex.Replace(args.Text, @"\s", "");
            value = value.Replace(Convert.ToChar(0x0).ToString(), "");
            if (value.Length > 0)
                tw.WriteLine("{0}\t{1}\t{2:HH:mm:ss.fff}", args.Item, value, DateTime.Now);
        }

        private static void OnDisconnected(object sender, DdeDisconnectedEventArgs args)
        {
            Console.WriteLine(
                "OnDisconnected: " +
                "IsServerInitiated=" + args.IsServerInitiated.ToString() + " " +
                "IsDisposed=" + args.IsDisposed.ToString());
        }

    }
}
