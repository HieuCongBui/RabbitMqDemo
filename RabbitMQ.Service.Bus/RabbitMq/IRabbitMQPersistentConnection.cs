using System;
using RabbitMQ.Client;
namespace RabbitMQ.Service.Bus.RabbitMq
{
    public interface IRabbitMQPersistentConnection
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel(); // IModel is part of RabbitMQ.Client namespace
    }
}
