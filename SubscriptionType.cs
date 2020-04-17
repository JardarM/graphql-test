using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;

namespace Jardar
{
    public class SubscriptionType
    {
        private readonly EventHubService eventHubService;
        public SubscriptionType(EventHubService eventHubService)
        {
            this.eventHubService = eventHubService;

        }

        public async ValueTask<IAsyncEnumerable<string>> TelemetryMessages(string id, CancellationToken token = default)
        {
            return eventHubService.Messages(id, token);
        }
    }
}
