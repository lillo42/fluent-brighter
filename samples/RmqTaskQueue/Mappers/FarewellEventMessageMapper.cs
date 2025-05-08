using System.Text.Json;

using Paramore.Brighter;

using RmqTaskQueue.Commands;

namespace RmqTaskQueue.Mappers;

public class FarewellEventMessageMapper : IAmAMessageMapper<FarewellEvent>
{
    public Message MapToMessage(FarewellEvent request)
    {
        var header = new MessageHeader(
            messageId: request.Id,
            topic: "farewell.event",
            messageType: MessageType.MT_EVENT,
            contentType: MessageBody.APPLICATION_JSON);

        var body = new MessageBody(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerialisationOptions.Options));
        var message = new Message(header, body);
        return message;
    }

    public FarewellEvent MapToRequest(Message message)
    {
        var farewellCommand = JsonSerializer.Deserialize<FarewellEvent>(message.Body.Bytes, JsonSerialisationOptions.Options)!;
        return farewellCommand;
    }
}
