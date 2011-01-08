using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDde.Client;

/*
 * List of topics:
 * BID, ASK, HIGH, LOW, 
 *
 */

namespace DDEConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("DDE Console");
            String _myapp = "TOS";
            String _topic = "LAST";
            String _item = "SPY";

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
                    client.StartAdvise(_item, 1, true, 60000);
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
        }

        private static void OnExecuteComplete(IAsyncResult ar)
        {
            try
            {
                DdeClient client = (DdeClient)ar.AsyncState;
                client.EndExecute(ar);
                Console.WriteLine("OnExecuteComplete");
            }
            catch (Exception e)
            {
                Console.WriteLine("OnExecuteComplete: " + e.Message);
            }
        }

        private static void OnPokeComplete(IAsyncResult ar)
        {
            try
            {
                DdeClient client = (DdeClient)ar.AsyncState;
                client.EndPoke(ar);
                Console.WriteLine("OnPokeComplete");
            }
            catch (Exception e)
            {
                Console.WriteLine("OnPokeComplete: " + e.Message);
            }
        }

        private static void OnRequestComplete(IAsyncResult ar)
        {
            try
            {
                DdeClient client = (DdeClient)ar.AsyncState;
                byte[] data = client.EndRequest(ar);
                Console.WriteLine("OnRequestComplete: " + Encoding.ASCII.GetString(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("OnRequestComplete: " + e.Message);
            }
        }

        private static void OnStartAdviseComplete(IAsyncResult ar)
        {
            try
            {
                DdeClient client = (DdeClient)ar.AsyncState;
                client.EndStartAdvise(ar);
                Console.WriteLine("OnStartAdviseComplete");
            }
            catch (Exception e)
            {
                Console.WriteLine("OnStartAdviseComplete: " + e.Message);
            }
        }

        private static void OnStopAdviseComplete(IAsyncResult ar)
        {
            try
            {
                DdeClient client = (DdeClient)ar.AsyncState;
                client.EndStopAdvise(ar);
                Console.WriteLine("OnStopAdviseComplete");
            }
            catch (Exception e)
            {
                Console.WriteLine("OnStopAdviseComplete: " + e.Message);
            }
        }

        private static void OnAdvise(object sender, DdeAdviseEventArgs args)
        {
            Console.WriteLine("OnAdvise: " + args.Text);
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
