using Paramore.Brighter;

namespace Fluent.Brighter.MySql.Tests;

public class MySqlDistributedLockBuilderTests
{
    private const string TestConnectionString = "Host=localhost;Username=test;Password=test;Database=testdb";

    [Fact]
    public void Configuration_WithRelationalDatabaseConfiguration_SetsValueCorrectly()
    {
        // Arrange
        var expectedConfig = new RelationalDatabaseConfiguration(TestConnectionString);
        var builder = new MySqlDistributedLockBuilder();

        // Act
        var result = builder.Configuration(expectedConfig);

        // Assert
        Assert.NotNull(result);
        Assert.Same(expectedConfig, result.GetType().GetField("_configuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(builder));
    }

    [Fact]
    public void Configuration_WithAction_SetsValueCorrectly()
    {
        // Arrange
        var builder = new MySqlDistributedLockBuilder();

        // Act
        var result = builder.Configuration(cfgBuilder => cfgBuilder
            .ConnectionString(TestConnectionString)
            .DatabaseName("TestDB")
            .BinaryMessagePayload(true));

        // Assert
        var actualConfig = (RelationalDatabaseConfiguration?)typeof(MySqlDistributedLockBuilder)
            .GetField("_configuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(builder);

        Assert.NotNull(result);
        Assert.NotNull(actualConfig);
        Assert.Equal(TestConnectionString, actualConfig!.ConnectionString);
        Assert.Equal("TestDB", actualConfig.DatabaseName);
        Assert.True(actualConfig.BinaryMessagePayload);
    }

    [Fact]
    public void ConfigurationIfMissing_WhenConfigurationIsNull_SetsValue()
    {
        // Arrange
        var defaultConfig = new RelationalDatabaseConfiguration("defaultConnectionString"); 
        var builder = new MySqlDistributedLockBuilder();

        // Act
        var result = builder.ConfigurationIfMissing(defaultConfig);

        // Assert
        var actualConfig = (RelationalDatabaseConfiguration?)typeof(MySqlDistributedLockBuilder)
            .GetField("_configuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(builder);

        Assert.NotNull(result);
        Assert.Same(defaultConfig, actualConfig);
    }

    [Fact]
    public void ConfigurationIfMissing_WhenConfigurationIsNotNull_DoesNotSetValue()
    {
        // Arrange
        var initialConfig = new RelationalDatabaseConfiguration("initialConnectionString");
        var defaultConfig = new RelationalDatabaseConfiguration("defaultConnectionString");
        
        var builder = new MySqlDistributedLockBuilder().Configuration(initialConfig);

        // Act
        var result = builder.ConfigurationIfMissing(defaultConfig);

        // Assert
        var actualConfig = (RelationalDatabaseConfiguration?)typeof(MySqlDistributedLockBuilder)
            .GetField("_configuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(builder);

        Assert.NotNull(result);
        Assert.Same(initialConfig, actualConfig);
    }

    [Fact]
    public void Build_CreatesMySqlLockingProviderWithCorrectOptions()
    {
        // Arrange
        var config = new RelationalDatabaseConfiguration(TestConnectionString);
        var builder = new MySqlDistributedLockBuilder().Configuration(config);

        // Act
        var provider = builder.Build();

        // Assuming MySqlLockingProvider exposes Options for testing
        var optionsField = provider.GetType().GetField("<connectionProvider>P", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var options = optionsField?.GetValue(provider);

        var connectionStringProperty = options?.GetType().GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var actualConnectionString = connectionStringProperty?.GetValue(options) as string;
            
        // Assert
        Assert.NotNull(provider);
        Assert.Equal(TestConnectionString, actualConnectionString); 
    } 
} 