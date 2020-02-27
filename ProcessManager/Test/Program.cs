using System;
using System.Diagnostics;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = Process.GetProcesses().Where(x=>x.ProcessName=="dotnet").OrderByDescending(x=>x.Id).ToList();
            foreach(var f1 in f)
            {
                f1.Kill();
                try
                {
                    f1.WaitForExit();
                }
                catch
                {
                }
            }
            f = Process.GetProcesses().Where(x => x.ProcessName == "cmd").OrderByDescending(x => x.Id).ToList();
            foreach (var f1 in f)
            {
                f1.Kill();
                try
                {
                    f1.WaitForExit();
                }
                catch
                {
                }
            }
            Console.ReadKey(true);
        }
    }
}
