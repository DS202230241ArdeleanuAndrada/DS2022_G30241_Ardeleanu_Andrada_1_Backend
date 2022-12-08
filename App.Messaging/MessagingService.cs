using App.Data;
using App.Hub;
using App.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace App.Messaging
{
    public class MessagingService : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly HttpClient client;
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<NotificationHubBasic> _hub;

        // initialize the connection, channel and queue 
        // inside the constructor to persist them 
        // for until the service (or the application) runs
        public MessagingService(IServiceProvider sp, IUnitOfWork uow, IHubContext<NotificationHubBasic> hub)
        {
            _sp = sp;

            _factory = new ConnectionFactory() { HostName = "host.docker.internal" };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "meteringQ",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            client = new HttpClient();
            _uow = uow;
            _hub = hub;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // when the service is stopping
            // dispose these references
            // to prevent leaks
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            // create a consumer that listens on the channel (queue)
            var consumer = new EventingBasicConsumer(_channel);

            // handle the Received event on the consumer
            // this is triggered whenever a new message
            // is added to the queue by the producer
            consumer.Received += (model, ea) =>
            {
                // read the message bytes
                var body = ea.Body.ToArray();

                // convert back to the original string
                // {index}|SuperHero{10000+index}|Fly,Eat,Sleep,Manga|1|{DateTime.UtcNow.ToLongDateString()}|0|0
                // is received here
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Received {0}", message);


                Task.Run(async () =>
                {
                    var model = JsonConvert.DeserializeObject<SensorModel>(message);

                    await SendMeasurements(model);
                    // BackgroundService is a Singleton service
                    // IHeroesRepository is declared a Scoped service
                    // by definition a Scoped service can't be consumed inside a Singleton
                    // to solve this, we create a custom scope inside the Singleton and 
                    // perform the insertion.
                    //using (var scope = _sp.CreateScope())
                    //{
                    //    var db = scope.ServiceProvider.GetRequiredService<IHeroesRepository>();
                    //    db.Create(hero);
                    //}
                });
            };

            _channel.BasicConsume(queue: "meteringQ", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task SendMeasurements(SensorModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var request = JsonConvert.DeserializeObject<AddDeviceMeasurementsRequest>(json);
            await _uow.UserDeviceRepository.AddDeviceMeasurements(request);
            _uow.Commit();

            var device = await _uow.UserDeviceRepository.GetDeviceById(request.DeviceId);
            var hourlyMeasurements = await _uow.UserDeviceRepository.GetHourlyMeasurements(request.DeviceId);
            var hourlyConsumption = hourlyMeasurements.Sum(m => m.MeasurementValue);
            if (hourlyConsumption > decimal.Parse(device.MaxConsumption))
            {
                await _hub.Clients.Groups(device.Username).SendAsync("ReceiveMessage", $"Device {device.Name} has passed the maximum hourly consumption!");
            }
            _uow.Commit();

            //var url = "https://172.17.0.1:8000/UserDevice/addMeasurements";
            //var response = await client.PostAsync(url, data);
        }
    }
}