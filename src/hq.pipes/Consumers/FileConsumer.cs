using System;
using System.IO;
using System.Threading.Tasks;
using hq.pipes.Serializers;

namespace hq.pipes.Consumers
{
    /// <summary>
    /// A consumer that handles an event and streams it to disk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileConsumer<T> : IConsume<T>
    {
        private readonly ISerializer _serializer;
        private readonly Func<string> _filenameFunc;

        public string BaseDirectory { get; }

        public FileConsumer(ISerializer serializer, string baseDirectory) : this(serializer, baseDirectory, null) { }

        public FileConsumer(string baseDirectory) : this(new JsonSerializer(), baseDirectory, null) { }

        public FileConsumer(string baseDirectory, Func<string> filenameFunc)  : this(new JsonSerializer(), baseDirectory, filenameFunc) { }
        
        public FileConsumer(ISerializer serializer, string baseDirectory, Func<string> filenameFunc)
        {
            _serializer = serializer;
            BaseDirectory = baseDirectory;
            _filenameFunc = filenameFunc ?? (() => $"{Guid.NewGuid()}.dat");
        }
        
        public async Task SaveAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var folder = BaseDirectory;
            var path = Path.Combine(folder, _filenameFunc());
            using(var file = File.OpenWrite(path))
            {
                await stream.CopyToAsync(file);
                await file.FlushAsync();    
            }
        }

        public async Task<bool> HandleAsync(T message)
        {
            try
            {
                var stream = _serializer.SerializeToStream(message);
                await SaveAsync(stream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}