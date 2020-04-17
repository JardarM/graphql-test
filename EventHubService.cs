using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.EventHubs;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Jardar {

    public class EventHubService {

        private readonly string EventHubConnectionString = "<<EventHubConnectionString>>";
        private readonly ILogger logger;
        PartitionReceiver receiver = null;

        public EventHubService(ILoggerFactory factory){
            logger = factory.CreateLogger<EventHubService>();
            Initializer().Wait();
        }

        public PartitionReceiver Receiver { get => receiver; set => receiver = value; }

        public async Task Initializer(){
            var c = EventHubClient.CreateFromConnectionString(EventHubConnectionString);
            var i = await c.GetRuntimeInformationAsync();
            var partitionId = i.PartitionIds.First();
            partitionId = "1";
            Receiver = c.CreateReceiver(PartitionReceiver.DefaultConsumerGroupName,partitionId, EventPosition.FromStart());
        }

        public async IAsyncEnumerable<string> Messages(string id, [EnumeratorCancellation] CancellationToken token = default){
            while ( ! token.IsCancellationRequested ){
                foreach ( var eventMessage in await Receiver.ReceiveAsync(10) ) {

                    yield return System.Text.Json.JsonSerializer.Serialize(eventMessage.Body);
                }
            }
        }
    }
}