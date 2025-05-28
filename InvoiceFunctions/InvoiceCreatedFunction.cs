using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Presentation.Services;

namespace InvoiceFunctions;



    public class InvoiceCreatedFunction(ILogger<InvoiceCreatedFunction> logger, IInvoiceService invoiceService)
    {
        private readonly ILogger<InvoiceCreatedFunction> _logger = logger;
        private readonly IInvoiceService _invoiceService = invoiceService;

        [Function(nameof(InvoiceCreatedFunction))]
        [ServiceBusOutput("email-service", Connection = "ServiceBusConnection")]
        public async Task<string?> Run([ServiceBusTrigger("invoice-created", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            var email = "johannafalkenmark@gmail.com";
            var invoiceId = message.Body?.ToString();
            if (!string.IsNullOrEmpty(invoiceId))
            {
                var emailRequest = await _invoiceService.GenerateInvoiceEmailAsync(invoiceId, email);
                if (emailRequest != null)
                {
                    await messageActions.CompleteMessageAsync(message);
                    _logger.LogInformation($"Invoice email request created for InvoiceId: {invoiceId}");
                    return JsonSerializer.Serialize(emailRequest);
                }
            }

            _logger.LogWarning("Received null or empty invoiceId from message.");
            await messageActions.DeadLetterMessageAsync(message, new Dictionary<string, object> { { "Reason", "InvoiceId is null or empty" } });
            return null;
        }
    }


