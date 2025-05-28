using Azure.Messaging.ServiceBus;
using System.Text.Json;
using System.Text;

namespace Presentation.Services;

public class InvoiceCreatedSenderService
{
    private readonly ServiceBusSender _sender;

    public InvoiceCreatedSenderService(IConfiguration configuration)
    {
        var client = new ServiceBusClient(configuration["ServiceBusConnection"]);
        _sender = client.CreateSender("invoice-created");
    }

    public async Task SendMessageAsync(string invoiceId)
    {
        var body = JsonSerializer.Serialize(invoiceId);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(body));
        await _sender.SendMessageAsync(message);
    }
}
