using System.Text.Json;

using Paramore.Brighter;
using Paramore.Brighter.JsonConverters;

using RmqTaskQueue.Commands;

namespace RmqTaskQueue.Mappers;

public class GreetingEventMessageMapper : IAmAMessageMapperAsync<GreetingEvent>
{
    public IRequestContext? Context { get; set; }
    
    public Task<Message> MapToMessageAsync(GreetingEvent request, Publication publication, CancellationToken ct = default)
    {
        var header = new MessageHeader(messageId: request.Id, topic: "greeting.event", messageType: MessageType.MT_EVENT);
        var body = new MessageBody(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerialisationOptions.Options));
        var message = new Message(header, body);
        return Task.FromResult(message);
    }

    public Task<GreetingEvent> MapToRequestAsync(Message message, CancellationToken ct = default)
    {
        var greetingCommand = JsonSerializer.Deserialize<GreetingEvent>(message.Body.Value, JsonSerialisationOptions.Options)!;
        return Task.FromResult(greetingCommand);
    }
}