using MassTransit;

namespace PlatformService.MassTransit
{
    public class MessagePublisher : BackgroundService
    {
        private readonly ILogger<MessagePublisher> _logger;
        private readonly IBus _bus;

        public MessagePublisher(ILogger<MessagePublisher> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _bus.Publish(new Message { Text = $"The time is {DateTimeOffset.Now}" });
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
