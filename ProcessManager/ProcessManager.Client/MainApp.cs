using CommandLine;
using PM.ClientConnection;
using PM.Connection;
using PM.Connection.Abstracts;
using PM.Connection.Commands.Requests;
using PM.Connection.Commands.Responses;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProcessManager.Client
{
    public class MainApp : IDisposable
    {
        private ClientConnect client;
        private ManualResetEvent manualResetEvent;
        private ManualResetEvent waitAnswer;
        private Dictionary<CommandType, Func<AppOptions, int>> actions;

        public MainApp()
        {
            Initialization();
        }

        private void Initialization()
        {
            actions = new Dictionary<CommandType, Func<AppOptions, int>>();
            actions.Add(CommandType.GetProcesses, GetProcesses);
            actions.Add(CommandType.Start, StartProcess);
            actions.Add(CommandType.Stop, StopProcess);
            actions.Add(CommandType.Restart, RestartProcess);

            client = new ClientConnect();
            client.Connect();
            client.CommandRecived += this.Client_CommandRecived;

            manualResetEvent = new ManualResetEvent(false);
            waitAnswer = new ManualResetEvent(false);
        }

        private void Client_CommandRecived(ClientConnect client, CommandBase command)
        {
            switch (command.CommandType)
            {
                case CommandTypes.GetProcessesResponse:
                    var cmdGet = command as CmdGetProcessesResponse;
                    Array.ForEach(cmdGet.Processes, x => 
                    {
                        Console.WriteLine(x);
                    });
                    break;

                default:
                    var cmdResp = command as CommandResponse;
                    if (cmdResp.Success)
                    {
                        Console.WriteLine("Операция выполнена успешно!");
                    }
                    else
                    {
                        Console.WriteLine("Не удалось выполнить операция!");
                    }

                    break;
            }
            waitAnswer.Set();
        }

        public int Start(string[] args)
        {
            return Parser.Default.ParseArguments<AppOptions>(args).MapResult(DataProcessing, ErrorProcessing);
        }

        private int DataProcessing(AppOptions options)
        {
            return actions[options.CommandType].Invoke(options);
        }

        private int ErrorProcessing(IEnumerable<Error> errors)
        {
            return 1;
        }

        private int GetProcesses(AppOptions options)
        {
            int result = 0;
            CmdGetProcessesRequest request = new CmdGetProcessesRequest(1);
            if (!client.SendCommand(request))
            {
                result = 1;
            }
            else
            {
                waitAnswer.WaitOne();
            }
            return result;
        }
        private int StartProcess(AppOptions options)
        {
            return CommandProcess(CommandTypes.StartProcessesRequest, options);
        }
        private int StopProcess(AppOptions options)
        {
            return CommandProcess(CommandTypes.KillProcesseRequest, options);
        }
        private int RestartProcess(AppOptions options)
        {
            return CommandProcess(CommandTypes.RestartProcesseRequest, options);
        }

        private int CommandProcess(CommandTypes commandType, AppOptions options)
        {
            int result = 0;

            ProcessModel model = new ProcessModel
            {
                ProcessId = options.ProcessId,
                PathExe = options.PathExe,
                ProcessName = options.ProcessName,
                Arguments = options.Arguments
            };

            CmdProcessRequest request = new CmdProcessRequest(1, commandType, model);
            if (!client.SendCommand(request))
            {
                result = 1;
            }
            else
            {
                waitAnswer.WaitOne();
            }

            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    client.CommandRecived -= this.Client_CommandRecived;
                    client.Dispose();
                    waitAnswer.Dispose();
                    manualResetEvent.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainApp()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
