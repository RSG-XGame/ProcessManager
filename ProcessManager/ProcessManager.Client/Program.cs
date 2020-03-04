using System;

namespace ProcessManager.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MainApp app = new MainApp())
            {
                app.Start(args);
            }
        }
    }
}
