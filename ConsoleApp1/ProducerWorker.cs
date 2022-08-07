﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka
{
    public class ProducerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> _logger;
        private readonly IConfiguration _config;
        private readonly string _host;
        private readonly string _topic;

        public ProducerWorker(ILogger<ProducerWorker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _host = _config.GetSection("Kafka:Host").Value;
            _topic = _config.GetSection("Kafka:Topic").Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var config = new ProducerConfig { BootstrapServers = _host };

                using (var producer = new ProducerBuilder<string, string>(config).Build())
                {
                    int i = 0;

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = producer.ProduceAsync(_topic,
                            new Message<string, string>
                            {
                                Key = $"KEY_{i}",
                                Value = Guid.NewGuid().ToString()
                            });
                        i++;

                        await Task.Delay(1000);
                    }
                }
            }
            catch (Exception) { throw; }

            await Task.CompletedTask;
        }
    }
}
