using System;
using System.IO;
using System.Threading.Tasks;

namespace hq.pipes.Consumers
{
    /// <summary>
    /// A consumer that handles a stream and saves it to disk
    /// </summary>
    public class StreamConsumer : IConsume<Stream>
    {
        private readonly string _baseDirectory;
        private readonly Func<string> _filenameFunc;

        public StreamConsumer(string baseDirectory) : this(baseDirectory, null) { }

        public StreamConsumer(string baseDirectory, Func<string> filenameFunc)
        {
            _baseDirectory = baseDirectory;
            _filenameFunc = filenameFunc ?? (() => $"{Guid.NewGuid()}.dat");
        }

        public async Task SaveAsync(Stream @event)
        {
            var folder = _baseDirectory;
            var path = Path.Combine(folder, _filenameFunc());
            using (var file = File.OpenWrite(path))
            {
                await @event.CopyToAsync(file);
            }
        }

        public async Task<bool> HandleAsync(Stream message)
        {
            try
            {
                await SaveAsync(message);
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}