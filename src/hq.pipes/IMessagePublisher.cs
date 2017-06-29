using System;
using System.Threading.Tasks;

namespace hq.pipes
{
    /// <summary>
    /// An message publisher allows eventing to one or more consumers.
    /// </summary>
    public interface IMessagePublisher : IDisposable
    {
        Task<bool> PublishAsync(object message);
        bool Publish(object message);
    }
}