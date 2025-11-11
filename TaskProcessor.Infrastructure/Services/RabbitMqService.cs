using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using TaskProcessor.Application;
using System.Threading.Tasks;
using TaskProcessor.Domain.Interfaces;
using System;
using Microsoft.Extensions.Options;
using TaskProcessor.Application.Models.Configuration;

public class RabbitMqService : IMessageQueue, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqService(IOptions<RabbitMqSettings> settings)
    {
        var config = settings.Value;

        _factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            UserName = "myuser",
            Password = "mypassword"
        };

        _connection = _factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync(string queueName, string message)
    {
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties();
        properties.ContentType = "application/json";

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }

    public async Task ConsumeAsync(string queueName, Func<string, Task> onMessageReceived)
    {
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await onMessageReceived(message);
        };

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
