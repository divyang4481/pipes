using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using hq.pipes.tests.Fakes;
using Xunit;
using Xunit.Abstractions;

namespace hq.pipes.tests
{
    public class PerformanceTests
    {
        private readonly ITestOutputHelper _output;

        public PerformanceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory, InlineData(10)]
        public async void Profile_message_handling_for_simple_handler(int seconds)
        {
            PerformanceEventHandler handler = new PerformanceEventHandler();

            var hub = new Hub();
            hub.Subscribe(handler);

            var random = new Random();
            var canceller = new CancellationTokenSource();
            var token = canceller.Token;

            var sw = Stopwatch.StartNew();
            var blaster = Task.Run(() =>
            {
                while(!canceller.IsCancellationRequested)
                {
                    hub.PublishAsync(new StringEvent(random.Next().ToString()));
                }
            }, token);
            
            await Task.Delay(TimeSpan.FromSeconds(1), token);

            canceller.Cancel();

            _output.WriteLine($"Handled {handler.HandledString} messages in {sw.Elapsed.TotalSeconds} seconds. ({handler.HandledString / sw.Elapsed.TotalSeconds:F2} msg/sec)");
        }

        public class PerformanceEventHandler : IConsume<StringEvent>, IConsume<IntegerEvent>
        {
            public int HandledString { get; private set; }
            public int HandledInteger { get; private set; }

            public Task<bool> HandleAsync(StringEvent message)
            {
                HandledString++;
                return Task.FromResult(true);
            }

            public Task<bool> HandleAsync(IntegerEvent message)
            {
                HandledInteger++;
                return Task.FromResult(true);
            }
        }
    }
}
