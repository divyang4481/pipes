using System;
using System.Threading.Tasks;

namespace hq.pipes.tests.Fakes
{
    public class ThrowingHandler : IConsume<IEvent>
    {
        public int Handled { get; private set; }

        public Task<bool> HandleAsync(IEvent message)
        {
            Handled++;
            throw new Exception();
        }
    }
}
