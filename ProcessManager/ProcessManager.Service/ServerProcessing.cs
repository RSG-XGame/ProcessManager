using PM.Connection;
using PM.Connection.Abstracts;
using PM.Connection.Commands.Requests;
using PM.Connection.Commands.Responses;
using PM.ServerConnection;
using ProcessManager.Logic.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Service
{
    public class ServerProcessing
    {
        private ServerLestener listener;
        private Dictionary<CommandTypes, MethodInfo> mapMethods;

        public ServerProcessing()
        {
            Initialization();
        }

        private void Initialization()
        {
            listener = new ServerLestener();
            listener.Initialization(Processing);
        
            mapMethods = new Dictionary<CommandTypes, MethodInfo>();
            mapMethods.Add(CommandTypes.GetProcessesRequest, GetType().GetMethod(nameof(GetProcesses)));
            mapMethods.Add(CommandTypes.StartProcessesRequest, GetType().GetMethod(nameof(StartProcess)));
            mapMethods.Add(CommandTypes.KillProcesseRequest, GetType().GetMethod(nameof(KillProcess)));
            mapMethods.Add(CommandTypes.RestartProcesseRequest, GetType().GetMethod(nameof(RestartProcess)));
        }

        public void Start()
        {
            listener.Listen();
        }

        public void Stop()
        {
            listener.Stop();
        }

        private CommandResponse Processing(CommandRequest request)
        {
            return (CommandResponse)mapMethods[request.CommandType].Invoke(this, new object[] { request });
        }
        
        private CmdGetProcessesResponse GetProcesses(CmdProcessRequest request)
        {
            return new CmdGetProcessesResponse(request.Id, false, ProcessHelper.GetProcessModels());
        }

        private CmdProcessResponse KillProcess(CmdProcessRequest request)
        {
            CmdProcessResponse response = null;
            try
            {
                ProcessController controller = new ProcessController(request.Model);
                if (!controller.StopProcess())
                {
                    throw new InvalidOperationException($"Не удалось остановить процесс {request.Model.ProcessId} {request.Model.PathExe}");
                }

                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, true);
            }
            catch
            {
                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, false);
            }
            return response;
        }

        private CmdProcessResponse StartProcess(CmdProcessRequest request)
        {
            CmdProcessResponse response = null;
            try
            {
                ProcessController controller = new ProcessController(request.Model);
                if (!controller.StartProcess())
                {
                    throw new InvalidOperationException($"Не удалось запустить процесс {request.Model.PathExe}");
                }

                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, true);
            }
            catch
            {
                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, false);
            }
            return response;
        }

        private CmdProcessResponse RestartProcess(CmdProcessRequest request)
        {
            CmdProcessResponse response = null;
            try
            {
                ProcessController controller = new ProcessController(request.Model);
                if (!controller.RestartProcess())
                {
                    throw new InvalidOperationException($"Не удалось перезапустить процесс {request.Model.PathExe}");
                }

                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, true);
            }
            catch
            {
                response = new CmdProcessResponse(request.Id, PM.Connection.CommandTypes.RestartProcesseResponse, false);
            }
            return response;
        }
    }
}
