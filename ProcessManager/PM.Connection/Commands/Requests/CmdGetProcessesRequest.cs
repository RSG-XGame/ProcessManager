using PM.Connection.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Requests
{
    [Serializable]
    public class CmdGetProcessesRequest : CommandRequest
    {
        public CmdGetProcessesRequest(long id) 
            : base(id, CommandTypes.GetProcessesRequest)
        {
        }
    }
}
