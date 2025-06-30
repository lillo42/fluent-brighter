using System.Net.Mime;
using System.Text.Json;

using Paramore.Brighter;
using Paramore.Brighter.JsonConverters;

using RmqTaskQueue.Commands;

namespace RmqTaskQueue.Mappers;

public class FarewellEventMessageMapper : IAmAMessageMapperAsync<FarewellEvent>
{
    public IRequestContext? Context { get; set; }
    
    public Task<Message> MapToMessageAsync(FarewellEvent request, Publication publication, CancellationToken ct = default)
    {
        var header = new MessageHeader(
            messageId: request.Id,
            topic: "farewell.event",
            messageType: MessageType.MT_EVENT,
            contentType: new ContentType("application/json"));

        var body = new MessageBody(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerialisationOptions.Options));
        var message = new Message(header, body);
        return Task.FromResult(message);
    }

    public Task<FarewellEvent> MapToRequestAsync(Message message, CancellationToken ct = default)
    {
        var farewellCommand = JsonSerializer.Deserialize<FarewellEvent>(message.Body.Bytes, JsonSerialisationOptions.Options)!;
        return Task.FromResult(farewellCommand);
    }
}
