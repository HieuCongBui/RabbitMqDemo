using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            Console.WriteLine("Chọn chế độ nhận tin nhắn:");
            Console.WriteLine("1. Nhận từ Queue trực tiếp (default exchange)");
            Console.WriteLine("2. Nhận từ Exchange (fanout - broadcast)");

            Console.Write("Lựa chọn (1/2): ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                const string queueName = "hello_queue";

                await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Nhận từ Queue '{queueName}': {message}");
                    await Task.Yield(); // Đảm bảo await để tương thích async
                };

                await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine($" [*] Đang chờ tin nhắn từ Queue '{queueName}'. Nhấn [Enter] để thoát.");
                Console.ReadLine();
            }
            else if (choice == "2")
            {
                const string exchangeName = "logs";
                await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);

                // Tạo queue tạm thời (server sẽ đặt tên tự động) cho mỗi consumer
                var queueName = (await channel.QueueDeclareAsync(queue: "", exclusive: true)).QueueName;

                await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: "");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Broadcast từ Exchange '{exchangeName}': {message}");
                    await Task.Yield();
                };

                await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine($" [*] Đang chờ broadcast từ Exchange '{exchangeName}'. Nhấn [Enter] để thoát.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ.");
            }

            Console.WriteLine("Consumer đã thoát.");
        }
    }
}
