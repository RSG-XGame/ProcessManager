using ProcessManager.Client.Commands;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProcessManager.Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ProcessModel selectedProcessModel;

        public ObservableCollection<ProcessModel> Processes { get; private set; }

        public ProcessModel SelectedProcessModel
        {
            get
            {
                return selectedProcessModel;
            }
            set
            {
                SetProperty(ref selectedProcessModel, value);
            }
        }

        public ICommand StartProcessCommand { get; private set; }
        public ICommand StopProcessCommand { get; private set; }
        public ICommand RestartProcessCommand { get; private set; }

        public MainViewModel()
            : base()
        {
            Initialize();
            testInit();
        }

        private void testInit()
        {
            var processes = Process.GetProcesses();
            
            foreach (var process in processes)
            {
                try
                {
                    Processes.Add(new ProcessModel
                    {
                        ProcessId = process.Id,
                        ProcessName = process.ProcessName,
                        PathExe = process.MainModule.FileName
                    });
                }
                catch
                {
                }
            }
        }

        private void Initialize()
        {
            Processes = new ObservableCollection<ProcessModel>();

            StartProcessCommand = new Command(StartProcess);
            StopProcessCommand = new Command(StopProcess, CanExecuteStopAndRestart);
            RestartProcessCommand = new Command(RestartProcess, CanExecuteStopAndRestart);
        }

        private void StartProcess()
        {
        }
        private void StopProcess()
        {
        }
        private void RestartProcess()
        {
        }

        private bool CanExecuteStopAndRestart()
        {
            return SelectedProcessModel != null;
        }
    }
}
