using PM.ClientConnection;
using PM.Connection.Commands.Requests;
using ProcessManager.Client.Commands;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProcessManager.Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ProcessModel selectedProcessModel;
        private DispatcherTimer dispatcherTimer;
        private ClientConnect client;

        private int requestId;
        private object requestLocker;

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
        }

        private void Initialize()
        {
            requestLocker = new object();
            requestId = 0;

            Processes = new ObservableCollection<ProcessModel>();

            StartProcessCommand = new Command(StartProcess);
            StopProcessCommand = new Command(StopProcess, CanExecuteStopAndRestart);
            RestartProcessCommand = new Command(RestartProcess, CanExecuteStopAndRestart);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            client.Connect();
            client.CommandRecived += Client_CommandRecived;
        }

        private void Client_CommandRecived(ClientConnect client, PM.Connection.Abstracts.CommandBase command)
        {
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CmdGetProcessesRequest request = new CmdGetProcessesRequest(GetRequestId());
            client.SendCommand(request);
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

        private int GetRequestId()
        {
            lock (requestLocker)
            {
                return ++requestId;
            }
        }
    }
}
