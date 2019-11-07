using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetKafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();

            var conf = new ConsumerConfig
            {
                GroupId = "hello-world-consumer",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                //EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            _consumer.Subscribe("hello-world-topic3");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _cancellationTokenSource.Cancel();
            };

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _logger.LogInformation("Consumindo mensagens...");
                    var consumeResult = _consumer.Consume(_cancellationTokenSource.Token);
                    _logger.LogInformation(
                        $"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Error occured: {e.Error.Reason}");
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }
    }
}
