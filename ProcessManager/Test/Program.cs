using PM.ClientConnection;
using PM.Connection.Abstracts;
using PM.Connection.Commands.Requests;
using PM.Connection.Commands.Responses;
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
                server.Initialization(Processing);
                server.Listen();

                Console.ReadKey();

                server.Stop();
            }
        }

        private static void Client_CommandRecived(ClientConnect client, CommandBase command)
        {
            Console.WriteLine($"Get response {command.Id} {command.CommandType}");
        }

        static CommandResponse Processing(CommandRequest request)
        {
            Console.WriteLine($"Get request {request.CommandType}");
            CommandResponse result = null;

            CmdGetProcessesResponse resp = new CmdGetProcessesResponse(1, true, ProcessHelper.GetProcessModels());
            result = resp;

            return result;
        }
    }
}
