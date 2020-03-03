using PM.Connection.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Requests
{
    public class CmdProcessRequest : CommandRequest
    {
        public CmdProcessRequest(long id, CommandTypes commandType) 
            : base(id, commandType)
        {
        }
    }
}
