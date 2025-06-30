using System.Reflection;

using Paramore.Brighter;

using Xunit;

namespace Fluent.Brighter.Postgres.Tests;

public class PostgresPublicationBuilderTests
{
    private readonly PostgresPublicationBuilder _builder = new();

    // Helper to access private fields via reflection
    private static T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)field?.GetValue(obj)!;
    }

    [Fact]
    public void SchemaName_SetsValueCorrectly()
    {
        const string schemaName = "test_schema";
        var result = _builder.SchemaName(schemaName);

        Assert.Equal(schemaName, GetPrivateField<string?>(_builder, "_schemaName"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void QueueStoreTable_SetsValueCorrectly()
    {
        const string queueTable = "custom_queue";
        var result = _builder.QueueStoreTable(queueTable);

        Assert.Equal(queueTable, GetPrivateField<string?>(_builder, "_queueStoreTable"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void BinaryMessagePayload_WhenTrue_SetsValue()
    {
        var result = _builder.BinaryMessagePayload(true);

        Assert.True(GetPrivateField<bool?>(_builder, "_binaryMessagePayload"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void BinaryMessagePayload_WhenFalse_SetsValue()
    {
        var result = _builder.BinaryMessagePayload(false);

        Assert.False(GetPrivateField<bool?>(_builder, "_binaryMessagePayload"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void BinaryMessagePayload_WhenNull_SetsValue()
    {
        var result = _builder.BinaryMessagePayload(null);

        Assert.Null(GetPrivateField<bool?>(_builder, "_binaryMessagePayload"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void EnableBinaryMessagePayload_SetsTrue()
    {
        var result = _builder.EnableBinaryMessagePayload();

        Assert.True(GetPrivateField<bool?>(_builder, "_binaryMessagePayload"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void DisableBinaryMessagePayload_SetsFalse()
    {
        var result = _builder.DisableBinaryMessagePayload();

        Assert.False(GetPrivateField<bool?>(_builder, "_binaryMessagePayload"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void CreateExchangeIfMissing_SetsOnMissingChannelCreate()
    {
        var result = _builder.CreateExchangeIfMissing();

        Assert.Equal(OnMissingChannel.Create, GetPrivateField<OnMissingChannel>(_builder, "_makeChannel"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void ValidateIfExchangeExists_SetsOnMissingChannelValidate()
    {
        var result = _builder.ValidateIfExchangeExists();

        Assert.Equal(OnMissingChannel.Validate, GetPrivateField<OnMissingChannel>(_builder, "_makeChannel"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void AssumeExchangeExists_SetsOnMissingChannelAssume()
    {
        var result = _builder.AssumeExchangeExists();

        Assert.Equal(OnMissingChannel.Assume, GetPrivateField<OnMissingChannel>(_builder, "_makeChannel"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Topic_SetsRoutingKey()
    {
        var routingKey = new RoutingKey("test.topic");
        var result = _builder.Topic(routingKey);

        Assert.Equal(routingKey, GetPrivateField<RoutingKey?>(_builder, "_topic"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void RequestType_WithType_SetsValue()
    {
        var requestType = typeof(string);
        var result = _builder.RequestType(requestType);

        Assert.Equal(requestType, GetPrivateField<Type?>(_builder, "_requestType"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void RequestType_Generic_SetsValue()
    {
        var result = _builder.RequestType<ExampleCommand>();

        Assert.Equal(typeof(ExampleCommand), GetPrivateField<Type?>(_builder, "_requestType"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void DataSchema_SetsUri()
    {
        var dataSchema = new Uri("http://example.com/schema");
        var result = _builder.DataSchema(dataSchema);

        Assert.Equal(dataSchema, GetPrivateField<Uri?>(_builder, "_dataSchema"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Source_DefaultValue_SetCorrectly()
    {
        var defaultSource = new Uri("http://goparamore.io");
        Assert.Equal(defaultSource, GetPrivateField<Uri>(_builder, "_source"));
    }

    [Fact]
    public void Source_CustomValue_SetsCorrectly()
    {
        var customSource = new Uri("http://custom.source");
        var result = _builder.Source(customSource);

        Assert.Equal(customSource, GetPrivateField<Uri>(_builder, "_source"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Source_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _builder.Source(null!));
    }

    [Fact]
    public void Subject_SetsValue()
    {
        const string subject = "test_subject";
        var result = _builder.Subject(subject);

        Assert.Equal(subject, GetPrivateField<string?>(_builder, "_subject"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Type_DefaultValue_SetCorrectly()
    {
        const string defaultType = "goparamore.io.Paramore.Brighter.Message";
        Assert.Equal(defaultType, GetPrivateField<string>(_builder, "_type"));
    }

    [Fact]
    public void Type_CustomValue_SetsCorrectly()
    {
        const string customType = "custom.event.type";
        var result = _builder.Type(customType);

        Assert.Equal(customType, GetPrivateField<string>(_builder, "_type"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Type_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _builder.Type(null!));
    }

    [Fact]
    public void CloudEventsAdditionalProperties_SetsDictionary()
    {
        var properties = new Dictionary<string, object> { { "key", "value" } };
        var result = _builder.CloudEventsAdditionalProperties(properties);

        Assert.Same(properties, GetPrivateField<Dictionary<string, object>?>(_builder, "_cloudEventsAdditionalProperties"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void ReplyTo_SetsQueueName()
    {
        const string replyTo = "reply_queue";
        var result = _builder.ReplyTo(replyTo);

        Assert.Equal(replyTo, GetPrivateField<string?>(_builder, "_replyTo"));
        Assert.Same(_builder, result);
    }

    [Fact]
    public void Build_CreatesPostgresPublicationWithCorrectValues()
    {
        var schemaName = "test_schema";
        var queueTable = "custom_queue";
        var binaryMessagePayload = true;
        var routingKey = new RoutingKey("test.topic");
        var requestType = typeof(string);
        var dataSchema = new Uri("http://example.com/schema");
        var source = new Uri("http://custom.source");
        var subject = "test_subject";
        var type = "custom.event.type";
        var cloudEventsProperties = new Dictionary<string, object> { { "key", "value" } };
        var replyTo = "reply_queue";

        var publication = _builder
            .SchemaName(schemaName)
            .QueueStoreTable(queueTable)
            .BinaryMessagePayload(binaryMessagePayload)
            .Topic(routingKey)
            .RequestType(requestType)
            .DataSchema(dataSchema)
            .Source(source)
            .Subject(subject)
            .Type(type)
            .CloudEventsAdditionalProperties(cloudEventsProperties)
            .ReplyTo(replyTo)
            .Build();

        Assert.Equal(schemaName, publication.SchemaName);
        Assert.Equal(queueTable, publication.QueueStoreTable);
        Assert.Equal(binaryMessagePayload, publication.BinaryMessagePayload);
        Assert.Equal(routingKey, publication.Topic);
        Assert.Equal(requestType, publication.RequestType);
        Assert.Equal(dataSchema, publication.DataSchema);
        Assert.Equal(source, publication.Source);
        Assert.Equal(subject, publication.Subject);
        Assert.Equal(type, publication.Type);
        Assert.Same(cloudEventsProperties, publication.CloudEventsAdditionalProperties);
        Assert.Equal(replyTo, publication.ReplyTo);
    }
    
    public class ExampleCommand(Guid id) : Command(id);
}