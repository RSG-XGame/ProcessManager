using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager.Client
{
    public enum CommandType
    {
        GetProcesses,
        Start,
        Stop,
        Restart,
    }

    public class AppOptions
    {
        [Option(longName: "processId", Required = false, HelpText = "id процесса")]
        public int ProcessId { get; set; }

        [Option(longName: "processName", Required = false, HelpText = "имя процесса")]
        public string ProcessName { get; set; }

        [Option(longName: "pathExe", Required = false, HelpText = "путь к исполняемому файлу")]
        public string PathExe { get; set; }

        [Option(longName: "arguments", Required = false, HelpText = "аргументы запуска (перезапуска) процесса")]
        public string Arguments { get; set; }

        [Option(longName: "commandType", Required = true, HelpText = "основная команда исполнения")]
        public CommandType CommandType { get; set; }
    }
}
