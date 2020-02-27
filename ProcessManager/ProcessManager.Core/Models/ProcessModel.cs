using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager.Core.Models
{
    public class ProcessModel
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string PathExe { get; set; }
        public string Arguments { get; set; }
    }
}
