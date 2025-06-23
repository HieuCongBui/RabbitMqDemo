

using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;


namespace RabbitMQ.Service.Bus.RabbitMq
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;
        private readonly object _lock = new object();
        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory,
            ILogger<RabbitMQPersistentConnection> logger,
            int retryCount = 5)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _retryCount = retryCount;
        }
        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMq Connections are available.");
            }
            return _connection.CreateModel();
        }
        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect...");
            lock (_lock)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                        });
                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{Host Name}' and is subscribed to failure events", _connectionFactory.ClientProvidedName);
                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ Client could not be created and opened.");
                    return false;
                }
            };
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ Client connection is blocked. Reason: {Reason}. Trying to re-connect...", e.Reason);

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogError(e.Exception, "RabbitMQ Client connection threw an exception. Trying to re-connect...");

            TryConnect();
        }
        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ Client connection is shutting down. Reason: {Reason}. Trying to re-connect...", e.ReplyText);

            TryConnect();
        }
    }
}
