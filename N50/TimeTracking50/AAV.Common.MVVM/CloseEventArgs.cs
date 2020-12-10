using System;

namespace MVVM.Common
{
    public class CloseEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }

    public class ClosedEventArgs : EventArgs
    {
        public bool ShutdownApplication { get; set; }
    }
}