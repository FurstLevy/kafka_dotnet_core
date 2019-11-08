using Confluent.Kafka;
using DotNetKafka.Producer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public KafkaController(ILogger<KafkaController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task Publicar([FromBody] KafkaViewModel viewModel)
        {
            var config = new ProducerConfig {BootstrapServers = _configuration.GetSection("KafkaServer").Value };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync("hello-world-topic3", new Message<Null, string> {Value = JsonSerializer.Serialize(viewModel)});

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
