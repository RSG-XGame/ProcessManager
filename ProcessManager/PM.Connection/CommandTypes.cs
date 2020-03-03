using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Connection
{
    public enum CommandTypes
    {
        GetProcessesRequest,
        KillProcesseRequest,
        RestartProcesseRequest,
        StartProcessesRequest,
        GetProcessesResponse,
        KillProcesseResponse,
        RestartProcesseResponse,
        StartProcessesResponse,
    }
}
