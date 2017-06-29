using System;
using System.Threading.Tasks;

namespace hq.pipes.tests.Fakes
{
    public class ErroringHandler : IConsume<ErrorEvent>
    {
        public Task<bool> HandleAsync(ErrorEvent message)
        {
            if(message.Error)
                throw new Exception("The message made me do it!");

            return Task.FromResult(true);
        }
    }
}