using System;

namespace hq.pipes
{
    public class ObservableOutcome
    {
        public bool Result { get; set; }
        public Exception Error { get; set; }
    }
}