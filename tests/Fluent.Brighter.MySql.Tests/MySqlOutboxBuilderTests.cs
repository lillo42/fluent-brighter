using System.Data.Common;

using Paramore.Brighter;
using Paramore.Brighter.Outbox.MySql;

namespace Fluent.Brighter.MySql.Tests;

public class MySqlOutboxBuilderTests
{
    private const string TestConnectionString = "Host=localhost;Username=test;Password=test;Database=testdb";

    [Fact]
    public void MaxOutStandingMessages_WhenSet_SetsValueCorrectly()
    {
        var builder = new MySqlOutboxBuilder();
        var result = builder.MaxOutStandingMessages(100);

        Assert.Equal(100, GetPrivateField<int?>(builder, "_maxOutStandingMessages"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void MaxOutStandingCheckInterval_WhenSet_SetsValueCorrectly()
    {
        var interval = TimeSpan.FromSeconds(10);
        var builder = new MySqlOutboxBuilder();
        var result = builder.MaxOutStandingCheckInterval(interval);

        Assert.Equal(interval, GetPrivateField<TimeSpan>(builder, "_maxOutStandingCheckInterval"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void BulkChunkSize_WhenSet_SetsValueCorrectly()
    {
        var builder = new MySqlOutboxBuilder();
        var result = builder.BulkChunkSize(50);

        Assert.Equal(50, GetPrivateField<int?>(builder, "_outboxBulkChunkSize"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void Bag_WhenSet_SetsValueCorrectly()
    {
        var bag = new Dictionary<string, object> { { "Key", "Value" } };
        var builder = new MySqlOutboxBuilder();
        var result = builder.Bag(bag);

        Assert.Same(bag, GetPrivateField<Dictionary<string, object>>(builder, "_outBoxBag"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void Timeout_WhenSet_SetsValueCorrectly()
    {
        var builder = new MySqlOutboxBuilder();
        var result = builder.Timeout(5000);

        Assert.Equal(5000, GetPrivateField<int?>(builder, "_outboxTimeout"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void Configuration_WithRelationalConfig_SetsValueCorrectly()
    {
        var config = new RelationalDatabaseConfiguration(TestConnectionString);
        var builder = new MySqlOutboxBuilder();
        var result = builder.Configuration(config);

        Assert.Same(config, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
        Assert.Same(builder, result);
    }

    [Fact]
    public void Configuration_WithAction_BuildsFromBuilder()
    {
        var builder = new MySqlOutboxBuilder();
        builder.Configuration(cfg => cfg.ConnectionString("newConn"));

        var config = GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration");
        Assert.NotNull(config);
        Assert.Equal("newConn", config.ConnectionString);
    }

    [Fact]
    public void ConfigurationIfIsMissing_WhenNull_SetsValue()
    {
        var defaultConfig = new RelationalDatabaseConfiguration("defaultConn");
        var builder = new MySqlOutboxBuilder();

        builder.ConfigurationIfIsMissing(defaultConfig);

        Assert.Same(defaultConfig, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
    }

    [Fact]
    public void ConfigurationIfIsMissing_WhenAlreadySet_DoesNotOverwrite()
    {
        var initialConfig = new RelationalDatabaseConfiguration("initialConn");
        var defaultConfig = new RelationalDatabaseConfiguration("defaultConn");

        var builder = new MySqlOutboxBuilder().Configuration(initialConfig);
        builder.ConfigurationIfIsMissing(defaultConfig);

        Assert.Same(initialConfig, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
    }

    [Fact]
    public void UseUnitOfWork_WhenTrue_SetsFlag()
    {
        var builder = new MySqlOutboxBuilder().UseUnitOfWork(true);
        Assert.True(GetPrivateField<bool>(builder, "_useUnitOfWork"));
    }

    [Fact]
    public void EnableUnitOfWork_SetsUseUnitOfWorkToTrue()
    {
        var builder = new MySqlOutboxBuilder().EnableUnitOfWork();
        Assert.True(GetPrivateField<bool>(builder, "_useUnitOfWork"));
    }

    [Fact]
    public void DisableUnitOfWork_SetsUseUnitOfWorkToFalse()
    {
        var builder = new MySqlOutboxBuilder().DisableUnitOfWork();
        Assert.False(GetPrivateField<bool>(builder, "_useUnitOfWork"));
    }

    [Fact]
    public void UnitOfWorkConnectionProvider_SetsProvider()
    {
        var mockProvider = new MockRelationalDbConnectionProvider();
        var builder = new MySqlOutboxBuilder().UnitOfWorkConnectionProvider(mockProvider);

        Assert.Same(mockProvider, GetPrivateField<IAmARelationalDbConnectionProvider>(builder, "_unitOfWork"));
    }

    [Fact]
    public void Build_ReturnsCorrectConfigurationWithDefaults()
    {
        var config = new RelationalDatabaseConfiguration(TestConnectionString);
        var builder = new MySqlOutboxBuilder().Configuration(config);

        var result = builder.Build();

        Assert.NotNull(result);
        Assert.Equal(-1, result.MaxOutStandingMessages);
        Assert.Equal(TimeSpan.Zero, result.MaxOutStandingCheckInterval);
        Assert.Null((object?)result.OutboxBulkChunkSize);
        Assert.Null((object?)result.OutboxTimeout);
        Assert.Null(result.OutBoxBag);
        Assert.IsType<MySqlOutbox>(result.Outbox);
    }

    [Fact]
    public void Build_ReturnsCorrectConfigurationWithCustomValues()
    {
        var config = new RelationalDatabaseConfiguration(TestConnectionString);
        var bag = new Dictionary<string, object> { { "Key", "Value" } };

        var builder = new MySqlOutboxBuilder()
            .Configuration(config)
            .MaxOutStandingMessages(100)
            .MaxOutStandingCheckInterval(TimeSpan.FromSeconds(5))
            .BulkChunkSize(50)
            .Timeout(30000)
            .Bag(bag)
            .EnableUnitOfWork()
            .UnitOfWorkConnectionProvider(new MockRelationalDbConnectionProvider());

        var result = builder.Build();

        Assert.Equal(100, result.MaxOutStandingMessages);
        Assert.Equal(TimeSpan.FromSeconds(5), result.MaxOutStandingCheckInterval);
        Assert.Equal(50, result.OutboxBulkChunkSize);
        Assert.Equal(30000, result.OutboxTimeout);
        Assert.Same(bag, result.OutBoxBag);
        Assert.IsType<MockRelationalDbConnectionProvider>(result.Outbox.GetType().GetField("_connectionProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(result.Outbox));
    }

    // Helper to access private fields via reflection
    private static T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field?.GetValue(obj)!;
    }
    
    // Mock implementation for testing
    public class MockRelationalDbConnectionProvider : IAmARelationalDbConnectionProvider
    {
        public DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

