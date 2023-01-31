using System.Reactive.Disposables;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Azure;
using Orders.Events;

namespace Orders.Messaging;

internal sealed class StorageQueueBus : IBus<PaymentApproved>, IAsyncObservable<PaymentApproved>
{
    private readonly QueueClient client;

    public StorageQueueBus(QueueClient client) => this.client = client;

    public Task Send(PaymentApproved message) 
        => client.SendMessageAsync(BinaryData.FromObjectAsJson(message));

    public IDisposable Subscribe(Func<PaymentApproved, Task> onNext)
    {
        bool listening = true;
        Run();

        return Disposable.Create(() => listening = false);

        async void Run()
        {
            await client.CreateIfNotExistsAsync();
            while (listening)
            {
                QueueMessage[] messages = await client.ReceiveMessagesAsync();
                foreach (QueueMessage message in messages)
                {
                    if (listening)
                    {
                        await onNext.Invoke(message.Body.ToObjectFromJson<PaymentApproved>());
                        await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    }
                }
            }
        }
    }
}