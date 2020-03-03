using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    Arguments = processModel.Arguments
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
                using (Process proc = Process.GetProcessById(processModel.ProcessId))
                {
                    proc.Kill();
                    proc.WaitForExit();
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
    }
}
