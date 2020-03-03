using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessManager.Logic.Logic
{
    public static class ProcessHelper
    {
        public static ProcessModel[] GetProcessModels()
        {
            return Process.GetProcesses(Environment.MachineName)
                .Select(x => new ProcessModel
                {
                    ProcessName = x.ProcessName,
                    ProcessId = x.Id,
                    PathExe = x.GetPathExe(),
                    Arguments = x.GetArguments()
                }
                ).ToArray();
        }

        private static string GetArguments(this Process process)
        {
            string result = string.Empty;
            try
            {
                result = process.StartInfo.Arguments;
            }
            catch
            {
            }
            return result;
        }
        private static string GetPathExe(this Process process)
        {
            string result = string.Empty;
            try
            {
                result = process.MainModule.FileName;
            }
            catch
            {
            }
            return result;
        }
    }
}
