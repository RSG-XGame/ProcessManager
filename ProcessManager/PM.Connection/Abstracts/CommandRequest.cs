using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Abstracts
{
    [Serializable]
    public abstract class CommandRequest : CommandBase
    {
        public CommandRequest(long id, CommandTypes commandType) 
            : base(id, commandType)
        {
        }
    }
}
