using System.Text;
using System.Text.Json;
using Asis.Framework.Monitoring.Interfaces;
using LGW.MessageDistributor.MessageBus.Core.Helpers.Monitoring.Params;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Asis.Framework.Monitoring.MessageBuses
{
    public sealed class RabbitMqMessageBus : IMessageBus, IOnMessageReceived, IDisposable
    {
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly object _syncLock = new object();

        private bool _disposed;

        // Basit bir event ile gelen mesajları "consume" edebiliriz.
        // Daha esnek yapılar için Delegate / Handler / Pipeline dizayn edebilirsiniz.
        // Örnek olarak "OnMessageReceived" event'i:
        public event Func<string, Task>? OnMessageReceivedAsync;

        public RabbitMqMessageBus(RabbitMqOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private void EnsureConnectionAndChannel()
        {
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                return;

            lock (_syncLock)
            {
                if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                    return;

                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    AutomaticRecoveryEnabled = _options.AutomaticRecoveryEnabled,
                    TopologyRecoveryEnabled = _options.TopologyRecoveryEnabled,
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                if (!string.IsNullOrWhiteSpace(_options.QueueName))
                {
                    _channel.QueueDeclare(
                        queue: _options.QueueName,
                        durable: _options.DurableQueue,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                }

                if (!string.IsNullOrWhiteSpace(_options.ExchangeName))
                {
                    _channel.ExchangeDeclare(
                        exchange: _options.ExchangeName,
                        type: _options.ExchangeType,
                        durable: true,
                        autoDelete: false,
                        arguments: null
                    );
                }

                _channel.BasicQos(
                    prefetchSize: 0,
                    prefetchCount: _options.PrefetchCount,
                    global: false
                );

                if (_options.EnablePublisherConfirms)
                {
                    _channel.ConfirmSelect();
                }

                Console.WriteLine($"[RabbitMqMessageBus] Connection/Channel established on {_options.HostName}:{_options.Port}, queue={_options.QueueName}, exchange={_options.ExchangeName}");
            }
        }

        public async Task SendAsync<T>(T message) where T : class
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqMessageBus));

            EnsureConnectionAndChannel();

            if (_channel == null)
                throw new InvalidOperationException("Channel is not available.");

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var props = _channel.CreateBasicProperties();
            if (_options.PersistentMessages)
            {
                props.Persistent = true;
            }

            string routingKey = string.IsNullOrWhiteSpace(_options.RoutingKey) ? _options.QueueName : _options.RoutingKey;

            _channel.BasicPublish(
                exchange: _options.ExchangeName ?? "",
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body
            );

            if (_options.EnablePublisherConfirms)
            {
                bool confirmed = _channel.WaitForConfirms(TimeSpan.FromSeconds(5));
                if (!confirmed)
                {
                    Console.WriteLine("[RabbitMqMessageBus] Publish not confirmed!");
                }
            }

            await Task.Yield();
            Console.WriteLine($"[RabbitMqMessageBus] Published message: {typeof(T).Name}");
        }

        public Task StartConsuming()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqMessageBus));

            EnsureConnectionAndChannel();

            if (_channel == null)
                throw new InvalidOperationException("Channel is not available.");

            // Consumer
            var consumer = SubscribeQueueConsuming();

            string queueName = _options.QueueName ?? throw new InvalidOperationException("QueueName must be specified.");
            _channel.BasicConsume(
                queue: queueName,
                autoAck: _options.AutoAck,
                consumer: consumer
            );

            Console.WriteLine(
                $"[RabbitMqMessageBus] Consumer started on queue='{queueName}'. AutoAck={_options.AutoAck}");
            return Task.CompletedTask;
        }

        private AsyncEventingBasicConsumer SubscribeQueueConsuming()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);

                    if (OnMessageReceivedAsync != null)
                    {
                        await OnMessageReceivedAsync.Invoke(messageJson);
                    }
                    else
                    {
                        Console.WriteLine($"[RabbitMqMessageBus] Received: {messageJson}");
                    }

                    if (!_options.AutoAck)
                    {
                        _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RabbitMqMessageBus] Exception in consumer: {ex.Message}");
                    if (!_options.AutoAck)
                    {
                        _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                }
            };
            return consumer;
        }

        public Task Consume()
        {
            return Task.CompletedTask;
        }

        public Task Consume<T>(T message)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (_channel != null && _channel.IsOpen)
                        _channel.Close();
                }
                catch (Exception)
                {
                    throw;
                }

                try
                {
                    if (_connection != null && _connection.IsOpen)
                        _connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            _disposed = true;
        }
    }
}