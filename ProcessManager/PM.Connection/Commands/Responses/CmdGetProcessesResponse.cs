using PM.Connection.Abstracts;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Responses
{
    public class CmdGetProcessesResponse : CommandResponse
    {
        private ProcessModel[] processes;

        public ProcessModel[] Processes => processes;

        public CmdGetProcessesResponse(long id, CommandTypes commandType, bool success,
            ProcessModel[] processes) 
            : base(id, commandType, success)
        {
            this.processes = processes;
        }
    }
}
