namespace Common.MessageBusServices
{
    public interface IAzureBusService
    {
        Task SendMessageAsync(string message);
        Task ReceiveMessageAsync();
    }
}
