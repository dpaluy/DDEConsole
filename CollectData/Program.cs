using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NDde.Client;
using System.Xml;
using System.Globalization;

namespace CollectData
{     
    class Program
    {
        enum DDE_VALUES { 
            TSHR, SFUT, SINT, IVVL, OPOS, POSP, AVRP, DTMR, DVOL, BLI1, BL1A, SLI1, SL1A, DLTA, TETA, VEGA, GAMA};

        private static TextWriter tw;

        static void Main(string[] args)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(@"tase.xml");
            }
            catch (Exception )
            {
                Console.WriteLine("File dde.xml was not found in current folder!");
                Console.WriteLine("Press ENTER to quit...");
                Console.ReadLine();
                return;
            }

            Dictionary<string, string> myDict = new Dictionary<string, string>();
            XmlNodeList nodeList;
            XmlElement root = xml.DocumentElement;
            nodeList = root.SelectNodes("/tase/option");
            foreach (XmlNode option in nodeList)
            {
                XmlNodeList optionsData = option.ChildNodes;
                string opname = "";
                string id = "";
                string exp = "";
                foreach (XmlNode op in optionsData)
                {
                    if (op.Name == "name")
                    {
                        opname = op.InnerText.Substring(0, 6);
                    }
                    else if (op.Name == "id")
                    {
                        id = op.InnerText;
                    }
                    else if (op.Name == "expiration")
                    {
                        DateTime dt = DateTime.ParseExact(op.InnerText, "dd/MM/yyyy",
                                   CultureInfo.InvariantCulture);
                        exp = dt.Month.ToString();
                    }
                }
                //Console.Write("{0} {1}\n", opname+"_"+exp, id);
                myDict.Add(opname + "_" + exp, id);
            }

            // create a writer and open the file
            string filename = "data" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + ".txt";
            tw = new StreamWriter(filename);

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
                    foreach (KeyValuePair<string, string> pair in myDict)
                    {
                        foreach(string name in Enum.GetNames(typeof(DDE_VALUES)))
                        {
                            string advice = name + pair.Value;
                            client.StartAdvise(advice, 1, true, 60000);
                        }
                    }
                    
                    client.Advise += OnAdvise;

                    // Wait for the user to press ENTER before proceding.
                    Console.WriteLine("Press Ctrl X to quit...");
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    while (info.Key != ConsoleKey.X && info.Modifiers != ConsoleModifiers.Control)
                    {
                        info = Console.ReadKey(true);
                    }
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
                tw.WriteLine("{0}\t{1}\t{2:dd/MM/yyyy HH:mm:ss.fff}", args.Item, value, DateTime.Now);
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
