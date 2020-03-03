using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Service
{
    public partial class Service1 : ServiceBase
    {
        private ServerProcessing processing;

        public Service1()
        {
            InitializeComponent();
            processing = new ServerProcessing();
        }

        protected override void OnStart(string[] args)
        {
            processing.Start();
        }

        protected override void OnStop()
        {
            processing.Stop();
        }
    }
}
