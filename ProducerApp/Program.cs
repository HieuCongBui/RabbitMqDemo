using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ProducerApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            Console.WriteLine("Chọn kiểu gửi tin nhắn:");
            Console.WriteLine("1. Gửi tới Queue trực tiếp (default exchange)");
            Console.WriteLine("2. Gửi tới Exchange (fanout - broadcast)");

            Console.Write("Lựa chọn (1/2): ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                // Direct to Queue
                const string queueName = "hello_queue";
                await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false);

                Console.WriteLine("Nhập tin nhắn để gửi tới Queue (nhập 'exit' để thoát):");
                while (true)
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();

                    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase)) break;

                    var message = $"[{DateTime.Now:HH:mm:ss}] {input}";
                    var body = Encoding.UTF8.GetBytes(message);

                    await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

                    Console.WriteLine($" [x] Đã gửi đến Queue '{queueName}': {message}");
                }
            }
            else if (choice == "2")
            {
                const string exchangeName = "logs";
                await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);

                Console.WriteLine("Nhập tin nhắn để broadcast (nhập 'exit' để thoát):");
                while (true)
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();

                    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase)) break;

                    var message = $"[{DateTime.Now:HH:mm:ss}] {input}";
                    var body = Encoding.UTF8.GetBytes(message);

                    await channel.BasicPublishAsync(exchange: exchangeName, routingKey: "", body: body);

                    Console.WriteLine($" [x] Đã broadcast đến Exchange '{exchangeName}': {message}");
                }
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ.");
            }

            Console.WriteLine("Producer đã thoát.");
        }
    }
}
