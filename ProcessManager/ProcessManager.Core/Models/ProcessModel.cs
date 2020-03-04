using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager.Core.Models
{
   [Serializable]
    public class ProcessModel
    {
        public int ProcessId { get; set; } = -1;
        public string ProcessName { get; set; }
        public string PathExe { get; set; }
        public string Arguments { get; set; }
        public string UserName { get; set; } = Environment.UserName;

        public override string ToString()
        {
            return $"{ProcessId, 6}\t{ProcessName, 32}\t{PathExe} {Arguments}";
        }
    }
}
