using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EventsManager.Infrastructure.MessageBrokers;

public sealed class RabbitMqMessageBroker : IMessageBroker
{
    private readonly ConnectionDetails _connection;
    private readonly ILogger<RabbitMqMessageBroker> _logger;

    public RabbitMqMessageBroker(
        ILogger<RabbitMqMessageBroker> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _connection=SetConnection(configuration);
    }

    public void SendMessage<TMessage>(TMessage message)
    {
        try
        {
            ProduceMessage(message);
        }

        catch (Exception ex)
        {
            _logger.LogError("Can't send message. {Message}",ex.Message);
        }
    }

    private void ProduceMessage<TMessage>(TMessage message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _connection.Host,
            Port=_connection.Port,
            UserName=_connection.User,
            Password=_connection.Password

        };

        var connection = factory.CreateConnection() ??
            throw new InvalidOperationException("Can't set connection to RabbitMQ server");

        using var channel = connection.CreateModel();
        channel.QueueDeclare(_connection.Queue, exclusive: false);
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _connection.Queue, body: body);
    }

    private ConnectionDetails SetConnection(IConfiguration configuration)
    {
        var details = new ConnectionDetails
        {
            Host=configuration["RabbitMQ:Host"],
            Queue=configuration["RabbitMQ:Queue"],
            User=configuration["RabbitMQ:User"],
            Password=configuration["RabbitMQ:Password"],
            Port=(int.TryParse(configuration["RabbitMQ:Port"], out int result)) ?
                result : throw new InvalidOperationException("Port value is incorrect")
        };

        return details;
    }
}
