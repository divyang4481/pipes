using System.Threading.Tasks;

namespace hq.pipes.tests.Fakes
{
    public class SuccessHandler : IConsume<IEvent>
    {
        public int Handled { get; private set; }

        public Task<bool> HandleAsync(IEvent message)
        {
            Handled++;
            return Task.FromResult(true);
        }
    }
}