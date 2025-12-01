using System;

using Paramore.Brighter.Firestore;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Firestore collection.
/// Provides methods to set collection name and TTL (Time To Live) settings.
/// </summary>
public sealed class FirestoreCollectionBuilder
{
    private string _name = string.Empty;

    /// <summary>
    /// Sets the name of the Firestore collection.
    /// </summary>
    /// <param name="name">The collection name</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreCollectionBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private TimeSpan? _ttl;

    /// <summary>
    /// Sets the TTL (Time To Live) for documents in the Firestore collection.
    /// Determines how long documents should be retained before automatic deletion.
    /// </summary>
    /// <param name="ttl">The time to live duration</param>
    /// <returns>The builder instance for method chaining</returns>
    public FirestoreCollectionBuilder SetTtl(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }

    /// <summary>
    /// Builds the FirestoreCollection instance with the configured options.
    /// </summary>
    /// <returns>A configured <see cref="FirestoreCollection"/> instance</returns>
    public FirestoreCollection Build()
    {
        return new FirestoreCollection
        {
            Name = _name,
            Ttl = _ttl
        };
    }
}

