using PM.ClientConnection;
using PM.ServerConnection;
using ProcessManager.Logic.Logic;
using System;
using System.Diagnostics;
using System.Linq;

namespace Test
{
    static class Program
    {
        static void Main(string[] args)
        {

            using (ServerLestener server = new ServerLestener())
            {
                server.Initialization();
                server.Listen();

                using (ClientConnect client = new ClientConnect())
                {
                    client.Connect();


                    Console.ReadKey();

                }

                server.Stop();

            }


        }
    }
}
