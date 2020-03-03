using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Abstracts
{
    [Serializable]
    public abstract class CommandResponse : CommandBase
    {
        private bool success;

        public bool Success => success;

        public CommandResponse(long id, CommandTypes commandType, bool success) 
            : base(id, commandType)
        {
            this.success = success;
        }
    }
}
