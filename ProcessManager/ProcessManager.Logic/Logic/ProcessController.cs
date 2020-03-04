using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessManager.Logic.Logic
{
    public class ProcessController
    {
        private ProcessModel processModel;

        public ProcessController(ProcessModel processModel)
        {
            this.processModel = processModel;
        }

        public bool StartProcess()
        {
            bool result = false;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = processModel.PathExe,
                    Arguments = processModel.Arguments,
                };

                using (Process newProcess = new Process { StartInfo = startInfo })
                {
                    result = newProcess.Start();
                }
            }
            catch
            {
            }
            return result; 
        }

        public bool StopProcess()
        {
            bool result = true;
            try
            {
                var model = CheckModel();

                if (model != null)
                {
                    using (Process proc = Process.GetProcessById(model.ProcessId))
                    {
                        proc.Kill();
                        proc.WaitForExit();
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool RestartProcess()
        {
            bool result = false;

            if (StopProcess())
            {
                result = StartProcess();
            }

            return result;
        }

        private ProcessModel CheckModel()
        {
            ProcessModel result = processModel;
            if (result.ProcessId == -1)
            {
                result = ProcessHelper.GetProcessModels()
                    .FirstOrDefault(x => x.PathExe == processModel.PathExe || x.ProcessName == processModel.ProcessName);
            }
            return result;
        }
    }
}
