using System;

namespace hq.pipes.Producers
{
    /// <summary>
    /// A base implementation for a producer that uses the default background producer as its worker thread.
    /// <remarks>
    /// See implementers for reference implementation; basically you subscribe a production to the Background directly in the constructor
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BackgroundProducer<T> : IProduce<T>, IDisposable
    {
        protected BackgroundThreadProducer<T> Background { get; private set; }

        protected BackgroundProducer()
        {
            Background = new BackgroundThreadProducer<T>();
        }

        public virtual void Attach(IConsume<T> consumer)
        {
            Background.Attach(consumer);
        }

        public virtual void Start(bool immediate = false)
        {
            Background.Start(immediate);
        }

        public virtual void Stop(bool immediate = false)
        {
            Background.Stop(immediate);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Background == null) return;
            Background.Dispose();
            Background = null;
        }
    }
}