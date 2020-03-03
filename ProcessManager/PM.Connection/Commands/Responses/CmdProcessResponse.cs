using PM.Connection.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Commands.Responses
{
    public class CmdProcessResponse : CommandResponse
    {
        public CmdProcessResponse(long id, CommandTypes commandType, bool success) 
            : base(id, commandType, success)
        {
        }
    }
}
