using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace FtpServiceSwitch.Events
{
    public class LoadProgressEvent : PubSubEvent<ProgressMessage>
    {
    }

    public class ProgressMessage
    {
        public int Progress { get; private set; }
        public string Message { get; private set; }

        public ProgressMessage(int progress, string message)
        {
            Progress = progress;
            Message = message;
        }
    }
}
