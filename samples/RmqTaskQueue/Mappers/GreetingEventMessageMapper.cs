using System.Text.Json;

using Paramore.Brighter;

using RmqTaskQueue.Commands;

namespace RmqTaskQueue.Mappers;

public class GreetingEventMessageMapper : IAmAMessageMapper<GreetingEvent>
{
    public Message MapToMessage(GreetingEvent request)
    {
        var header = new MessageHeader(messageId: request.Id, topic: "greeting.event", messageType: MessageType.MT_EVENT);
        var body = new MessageBody(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerialisationOptions.Options));
        var message = new Message(header, body);
        return message;
    }

    public GreetingEvent MapToRequest(Message message)
    {
        var greetingCommand = JsonSerializer.Deserialize<GreetingEvent>(message.Body.Value, JsonSerialisationOptions.Options)!;
        return greetingCommand;
    }
}