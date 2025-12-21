namespace F1Simulator.RaceControlService.Messaging.Interfaces
{
    public interface IPublishService
    {
        Task PublishAsync(object data, string routingKey);
    }
}
