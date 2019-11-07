using Confluent.Kafka;
using DotNetKafka.Producer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetKafka.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaController : ControllerBase
    {
        private readonly ILogger<KafkaController> _logger;

        public KafkaController(ILogger<KafkaController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task Publicar([FromBody] KafkaViewModel viewModel)
        {
            var config = new ProducerConfig {BootstrapServers = "localhost:9092"};

            Action<DeliveryReport<Null, string>> handler = r =>
                _logger.LogInformation(!r.Error.IsError ? $"To {r.TopicPartitionOffset}" : $"Error {r.Error.Reason}");

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync("hello-world-topic3", new Message<Null, string> {Value = JsonSerializer.Serialize(viewModel)});

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
