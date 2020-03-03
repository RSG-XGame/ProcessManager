using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection.Abstracts
{
    public abstract class CommandBase
    {
        private long id;
        private CommandTypes commandType;
        public long Id => id;
        public CommandTypes CommandType => commandType;

        public CommandBase(long id, CommandTypes commandType)
        {
            this.id = id;
            this.commandType = commandType;
        }
    }
}
