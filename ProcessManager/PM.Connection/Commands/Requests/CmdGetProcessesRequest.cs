using PM.Connection.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Requests
{
    public class CmdGetProcessesRequest : CommandRequest
    {
        public CmdGetProcessesRequest(long id, CommandTypes commandType) 
            : base(id, commandType)
        {
        }
    }
}
