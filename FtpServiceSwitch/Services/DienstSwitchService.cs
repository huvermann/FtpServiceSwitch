using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Prism.Events;

namespace FtpServiceSwitch.Services
{
    

    public interface IDienstSwitchService
    {
        bool ServiceIsRunning { get; set; }
    }

    public class DienstSwitchService : IDienstSwitchService
    {
        const string serviceName = "FileZilla Server FTP server";
        ServiceController _controller;
        IEventAggregator _eventAggregator;

        public DienstSwitchService(IEventAggregator eventAggregator)
        {
            _controller = new ServiceController(serviceName);
            _eventAggregator = eventAggregator;
        }

        private bool _serviceIsRunning;

        public bool ServiceIsRunning
        {
            get {
                _serviceIsRunning = (_controller.Status == ServiceControllerStatus.Running);
                
                return _serviceIsRunning;
            }
            set {
                _serviceIsRunning = value;
                if (value == true)
                {
                    if (_controller.Status == ServiceControllerStatus.Stopped)
                    {
                        _controller.Start();
                        _controller.WaitForStatus(ServiceControllerStatus.Running);
                    }
                    
                } else
                {
                    if (_controller.Status == ServiceControllerStatus.Running)
                    {
                        _controller.Stop();
                        _controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                        
                }
                Thread.Sleep(5000);
            }
        }



    }
}
