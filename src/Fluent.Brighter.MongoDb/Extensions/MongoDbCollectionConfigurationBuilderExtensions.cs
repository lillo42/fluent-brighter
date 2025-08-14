using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class MongoDbCollectionConfigurationBuilderExtensions
{
    #region Make Collection
    public static MongoDbCollectionConfigurationBuilder CreateCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
        => build.SetMakeCollection(OnResolvingACollection.CreateIfNotExists);
    
    public static MongoDbCollectionConfigurationBuilder ValidateCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
        => build.SetMakeCollection(OnResolvingACollection.Validate);

    public static MongoDbCollectionConfigurationBuilder AssumeCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
        => build.SetMakeCollection(OnResolvingACollection.Assume);
    #endregion
}