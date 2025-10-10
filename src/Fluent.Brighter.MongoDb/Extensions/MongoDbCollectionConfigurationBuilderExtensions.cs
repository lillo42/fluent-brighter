using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="MongoDbCollectionConfigurationBuilder"/> to simplify
/// configuration of collection resolution behavior when the target MongoDB collection may not exist.
/// </summary>
public static class MongoDbCollectionConfigurationBuilderExtensions
{
    #region Make Collection

    /// <summary>
    /// Configures the collection to be automatically created if it does not already exist.
    /// This is the default and recommended behavior for most Brighter scenarios (e.g., outbox, inbox).
    /// </summary>
    /// <param name="build">The <see cref="MongoDbCollectionConfigurationBuilder"/> to extend.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static MongoDbCollectionConfigurationBuilder CreateCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
    {
        return build.SetMakeCollection(OnResolvingACollection.CreateIfNotExists);
    }

    /// <summary>
    /// Configures the system to validate that the collection exists and has the expected schema/indexes,
    /// but will not create it if missing. Throws an exception if the collection is absent or invalid.
    /// </summary>
    /// <param name="build">The <see cref="MongoDbCollectionConfigurationBuilder"/> to extend.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static MongoDbCollectionConfigurationBuilder ValidateCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
    {
        return build.SetMakeCollection(OnResolvingACollection.Validate);
    }

    /// <summary>
    /// Assumes the collection already exists and skips any creation or validation logic.
    /// Use this option only when you manage collection lifecycle externally (e.g., via infrastructure-as-code).
    /// </summary>
    /// <param name="build">The <see cref="MongoDbCollectionConfigurationBuilder"/> to extend.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static MongoDbCollectionConfigurationBuilder AssumeCollectionIfNotExists(
        this MongoDbCollectionConfigurationBuilder build)
    {
        return build.SetMakeCollection(OnResolvingACollection.Assume);
    }

    #endregion
}