using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using HotChocolate.Types;

namespace Demo.Accounts
{
    public class Subscription
    {
        [SubscribeAndResolve]
        public async ValueTask<IAsyncEnumerable<int>> Count(CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<int>();
            await Task.Yield();

            async void CountAsync()
            {
                try
                {
                    var counter = 0;
                    while (counter < 10 && !cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(100, cancellationToken);
                        await channel.Writer.WriteAsync(++counter, cancellationToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    // Do nothing
                }

                channel.Writer.Complete();
            }

            CountAsync();

            return channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}