namespace Fluent.Brighter;

public static class PostgresMessageProducerFactoryBuilderExtensions
{
    public static RelationalDatabaseConfigurationBuilder EnableBinaryMessagePayload(
        this RelationalDatabaseConfigurationBuilder builder)
        => builder.SetBinaryMessagePayload(true);
    
    public static RelationalDatabaseConfigurationBuilder DisableBinaryMessagePayload(
        this RelationalDatabaseConfigurationBuilder builder)
        => builder.SetBinaryMessagePayload(false);
}