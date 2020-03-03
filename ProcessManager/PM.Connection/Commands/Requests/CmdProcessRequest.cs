using PM.Connection.Abstracts;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Requests
{
    [Serializable]
    public class CmdProcessRequest : CommandRequest
    {
        private ProcessModel model;

        public ProcessModel Model => model;

        public CmdProcessRequest(long id, CommandTypes commandType, ProcessModel model) 
            : base(id, commandType)
        {
            this.model = model;
        }
    }
}
