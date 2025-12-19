namespace F1Simulator.RaceControlService.Messaging
{
    public interface IPublishService
    {
        Task Publish(object data, string routingKey);
    }
}
