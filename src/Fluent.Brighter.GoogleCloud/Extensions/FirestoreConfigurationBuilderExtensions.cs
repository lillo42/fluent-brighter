using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="FirestoreConfigurationBuilder"/> to provide convenient configuration
/// of Firestore collections using builder pattern callbacks.
/// </summary>
public static class FirestoreConfigurationBuilderExtensions
{
    /// <summary>
    /// Sets the inbox Firestore collection name.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="tableName">The name of the inbox collection</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetInbox(
        this FirestoreConfigurationBuilder builder, 
        string tableName)
    {
        return builder.SetInbox(c => c.SetName(tableName));
    }
    
    /// <summary>
    /// Sets the inbox Firestore collection using a fluent configuration callback.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="configure">An action to configure the inbox collection builder</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetInbox(
        this FirestoreConfigurationBuilder builder, 
        Action<FirestoreCollectionBuilder> configure)
    {
        var collection = new FirestoreCollectionBuilder();
        configure(collection);
        return builder.SetInbox(collection.Build());
    }
    
    /// <summary>
    /// Sets the outbox Firestore collection name.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="tableName">The name of the outbox collection</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetOutbox(
        this FirestoreConfigurationBuilder builder, 
        string tableName)
    {
        return builder.SetOutbox(c => c.SetName(tableName));
    }
    
    /// <summary>
    /// Sets the outbox Firestore collection using a fluent configuration callback.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="configure">An action to configure the outbox collection builder</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetOutbox(
        this FirestoreConfigurationBuilder builder, 
        Action<FirestoreCollectionBuilder> configure)
    {
        var collection = new FirestoreCollectionBuilder();
        configure(collection);
        return builder.SetOutbox(collection.Build());
    }
    
    /// <summary>
    /// Sets the locking Firestore collection name.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="tableName">The name of the locking collection</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetLocking(
        this FirestoreConfigurationBuilder builder, 
        string tableName)
    {
        return builder.SetLocking(c => c.SetName(tableName));
    }
    
    /// <summary>
    /// Sets the locking Firestore collection using a fluent configuration callback.
    /// </summary>
    /// <param name="builder">The Firestore configuration builder instance</param>
    /// <param name="configure">An action to configure the locking collection builder</param>
    /// <returns>The Firestore configuration builder instance for method chaining</returns>
    public static FirestoreConfigurationBuilder SetLocking(
        this FirestoreConfigurationBuilder builder, 
        Action<FirestoreCollectionBuilder> configure)
    {
        var collection = new FirestoreCollectionBuilder();
        configure(collection);
        return builder.SetLocking(collection.Build());
    }
}