using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using FtpServiceSwitch.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using Prism.Events;
using FtpServiceSwitch.Events;
using System.Diagnostics;

namespace FtpServiceSwitch.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IDienstSwitchService dienstSwitchService;
        private IEventAggregator eventAggregator;
        private const string FTPScannFolder = @"D:\Daten\Scanner";

        public MainWindowViewModel(IDienstSwitchService dienstSwitchService, IEventAggregator eventAggregator)
        {
            this.dienstSwitchService = dienstSwitchService;
            _buttonContent = "Change service state";
            _statusMessage = "Loading...";
            _workingInProgress = false;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<LoadProgressEvent>().Subscribe(OnLoadProgress);
            this.OnButttonChangeState = new DelegateCommand(async () => await buttonClickAsync(), CanExecuteStateChange);
            this.OnOpenScannerFolder = new DelegateCommand(async () => await openScannerFolderAsync(), CanExecuteStateChange);
            this.updateButtonContentAsync();
            this.updateStatusMessageAsync();
            
        }

       

        private void OnLoadProgress(ProgressMessage progress)
        {
            Console.WriteLine(string.Format("Progress {0}%: {1}", progress.Progress, progress.Message));
        }

        private bool CanExecuteStateChange()
        {
            return (this._workingInProgress == false);
        }

        private bool _workingInProgress;

        public bool WorkingInProgress
        {
            get { return _workingInProgress; }
            set {
                SetProperty(ref _workingInProgress, value);
                DelegateCommand dele = new DelegateCommand(async () => await buttonClickAsync(), CanExecuteStateChange);
                dele.RaiseCanExecuteChanged();

                ((DelegateCommand) OnButttonChangeState).RaiseCanExecuteChanged();
            }
        }

        async Task updateStatusMessageAsync()
        {
            await Task.Run(() => {

                if (this.dienstSwitchService.ServiceIsRunning)
                {
                    this.StatusMessage = "Service is running!";
                }
                else
                {
                    this.StatusMessage = "Service is stopped!";
                }
            });
        }

        async Task updateButtonContentAsync()
        {
            await Task.Run(() =>
            {
                this.WorkingInProgress = true;
                if (this.dienstSwitchService.ServiceIsRunning)
                {
                    this.ButtonContent = "Stop service";
                }
                else
                {
                    this.ButtonContent = "Run Service";
                }
                this.WorkingInProgress = false;
            });
        }

        private async Task buttonClickAsync()
        {
            await Task.Run(() =>
             {
                 this.WorkingInProgress = true;
                 this.ButtonContent = "Working...";
                 if (this.dienstSwitchService.ServiceIsRunning)
                 {
                     this.StatusMessage = "Stopping service ...";
                     this.dienstSwitchService.ServiceIsRunning = false;
                     this.StatusMessage = "Service stopped!";
                 }
                 else
                 {
                     this.StatusMessage = "Starting service...";
                     this.dienstSwitchService.ServiceIsRunning = true;
                     this.StatusMessage = "Service started!";
                 }

                     updateButtonContentAsync();
                 this.WorkingInProgress = false;
             });
            
        }

        private async Task openScannerFolderAsync()
        {
            await Task.Run(() =>
            {
                this.WorkingInProgress = true;
                Process.Start(FTPScannFolder);
                this.WorkingInProgress = false;
            });
            
        }

        private bool _isChecked = true;

        private async Task changeState(bool state)
        {
            this.dienstSwitchService.ServiceIsRunning = state;
        }
        

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        private string _buttonContent;

        public string ButtonContent
        {
            get { return _buttonContent; }
            set { SetProperty(ref _buttonContent, value); }
        }

        public ICommand OnButttonChangeState { get; private set; }
        public ICommand OnOpenScannerFolder { get; private set; }


    }
}
