using System;

using Paramore.Brighter.MessagingGateway.Redis;

namespace Fluent.Brighter.Redis;

/// <summary>
/// Builder class for configuring Redis messaging gateway settings.
/// This builder provides a fluent API for setting connection timeouts, pool sizes, retry behavior,
/// and other Redis-specific configuration options.
/// </summary>
public sealed class RedisMessagingGatewayConfigurationBuilder
{
    private int? _defaultConnectTimeout;
    
    /// <summary>
    /// Sets the default RedisClient socket connect timeout.
    /// </summary>
    /// <param name="defaultConnectTimeout">The connect timeout in milliseconds, or null for no timeout (default -1, None).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDefaultConnectTimeout(int? defaultConnectTimeout)
    {
        _defaultConnectTimeout = defaultConnectTimeout;
        return this;
    }
    
    private int? _defaultSendTimeout;
    
    /// <summary>
    /// Sets the default RedisClient socket send timeout.
    /// </summary>
    /// <param name="defaultSendTimeout">The send timeout in milliseconds, or null for no timeout (default -1, None).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDefaultSendTimeout(int? defaultSendTimeout)
    {
        _defaultSendTimeout = defaultSendTimeout;
        return this;
    }
    
    private int? _defaultReceiveTimeout;
    
    /// <summary>
    /// Sets the default RedisClient socket receive timeout.
    /// </summary>
    /// <param name="defaultReceiveTimeout">The receive timeout in milliseconds, or null for no timeout (default -1, None).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDefaultReceiveTimeout(int? defaultReceiveTimeout)
    {
        _defaultReceiveTimeout = defaultReceiveTimeout;
        return this;
    }
    
    private int? _defaultIdleTimeOutSecs;
    
    /// <summary>
    /// Sets the default idle timeout before a subscription is considered stale.
    /// </summary>
    /// <param name="defaultIdleTimeOutSecs">The idle timeout in seconds, or null to use the default (240 seconds).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDefaultIdleTimeOutSecs(int? defaultIdleTimeOutSecs)
    {
        _defaultIdleTimeOutSecs = defaultIdleTimeOutSecs;
        return this;
    }
    
    private int? _defaultRetryTimeout;
    
    /// <summary>
    /// Sets the default retry timeout for automatic retry of failed operations.
    /// </summary>
    /// <param name="defaultRetryTimeout">The retry timeout in milliseconds, or null to use the default (10,000ms).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDefaultRetryTimeout(int? defaultRetryTimeout)
    {
        _defaultRetryTimeout = defaultRetryTimeout;
        return this;
    }
    
    private int? _bufferPoolMaxSize;
    
    /// <summary>
    /// Sets the byte buffer size for operations to use a byte buffer pool.
    /// </summary>
    /// <param name="bufferPoolMaxSize">The buffer pool maximum size in bytes, or null to use the default (500kb).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetBufferPoolMaxSize(int? bufferPoolMaxSize)
    {
        _bufferPoolMaxSize = bufferPoolMaxSize;
        return this;
    }
    
    private bool? _verifyMasterConnections;
    
    /// <summary>
    /// Sets whether connections to master hosts should be verified to ensure they're still master instances.
    /// </summary>
    /// <param name="verifyMasterConnections">True to verify master connections, false to skip verification, or null to use the default (true).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetVerifyMasterConnections(bool? verifyMasterConnections)
    {
        _verifyMasterConnections = verifyMasterConnections;
        return this;
    }
    
    private int? _hostLookupTimeoutMs;
    
    /// <summary>
    /// Sets the connect timeout on clients used to find the next available host.
    /// </summary>
    /// <param name="hostLookupTimeoutMs">The host lookup timeout in milliseconds, or null to use the default (200ms).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetHostLookupTimeoutMs(int? hostLookupTimeoutMs)
    {
        _hostLookupTimeoutMs = hostLookupTimeoutMs;
        return this;
    }
    
    private int? _assumeServerVersion;
    
    /// <summary>
    /// Sets the assumed Redis server version to skip server version checks.
    /// Specify the minimum version number (e.g., 2.8.12 => 2812, 2.9.1 => 2910).
    /// </summary>
    /// <param name="assumeServerVersion">The assumed server version number, or null to perform version checks.</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetAssumeServerVersion(int? assumeServerVersion)
    {
        _assumeServerVersion = assumeServerVersion;
        return this;
    }
    
    private TimeSpan? _deactivatedClientsExpiry;
    
    /// <summary>
    /// Sets how long to hold deactivated clients before disposing their subscription.
    /// Use <see cref="TimeSpan.Zero"/> to dispose of deactivated clients immediately.
    /// </summary>
    /// <param name="deactivatedClientsExpiry">The expiry time for deactivated clients, or null to use the default (1 minute).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDeactivatedClientsExpiry(TimeSpan? deactivatedClientsExpiry)
    {
        _deactivatedClientsExpiry = deactivatedClientsExpiry;
        return this;
    }
    
    private bool? _disableVerboseLogging;
    
    /// <summary>
    /// Sets whether debug logging should log detailed Redis operations.
    /// </summary>
    /// <param name="disableVerboseLogging">True to disable verbose logging, false to enable it, or null to use the default (false).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetDisableVerboseLogging(bool? disableVerboseLogging)
    {
        _disableVerboseLogging = disableVerboseLogging;
        return this;
    }
    
    private int? _backoffMultiplier;
    
    /// <summary>
    /// Sets the exponential backoff interval for retrying connections on socket failure.
    /// </summary>
    /// <param name="backoffMultiplier">The backoff multiplier in milliseconds, or null to use the default (10ms).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetBackoffMultiplier(int? backoffMultiplier)
    {
        _backoffMultiplier = backoffMultiplier;
        return this;
    }
    
    private int? _maxPoolSize;
    
    /// <summary>
    /// Sets the maximum size of the Redis connection pool.
    /// </summary>
    /// <param name="maxPoolSize">The maximum pool size, or null for no limit (default None).</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetMaxPoolSize(int? maxPoolSize)
    {
        _maxPoolSize = maxPoolSize;
        return this;
    }
    
    private TimeSpan? _messageTimeToLive;
    
    /// <summary>
    /// Sets how long message bodies persist in Redis before being reclaimed.
    /// Once reclaimed, attempts to retrieve the message will fail and the message will be rejected.
    /// </summary>
    /// <param name="messageTimeToLive">The time-to-live for messages, or null to use the default.</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetMessageTimeToLive(TimeSpan? messageTimeToLive)
    {
        _messageTimeToLive = messageTimeToLive;
        return this;
    }
    
    private string? _redisConnectionString;
    
    /// <summary>
    /// Sets the Redis connection string that defines how to connect to Redis.
    /// </summary>
    /// <param name="redisConnectionString">The Redis connection string, or null if not specified.</param>
    /// <returns>The current <see cref="RedisMessagingGatewayConfigurationBuilder"/> instance for method chaining.</returns>
    public RedisMessagingGatewayConfigurationBuilder SetRedisConnectionString(string? redisConnectionString)
    {
        _redisConnectionString = redisConnectionString;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="RedisMessagingGatewayConfiguration"/> instance.
    /// This method is called internally to create the configuration with all the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="RedisMessagingGatewayConfiguration"/> instance.</returns>
    internal RedisMessagingGatewayConfiguration Build()
    {
        return new RedisMessagingGatewayConfiguration
        {
            DefaultConnectTimeout = _defaultConnectTimeout,
            DefaultSendTimeout = _defaultSendTimeout,
            DefaultReceiveTimeout = _defaultReceiveTimeout,
            DefaultIdleTimeOutSecs = _defaultIdleTimeOutSecs,
            DefaultRetryTimeout = _defaultRetryTimeout,
            BufferPoolMaxSize = _bufferPoolMaxSize,
            VerifyMasterConnections = _verifyMasterConnections,
            HostLookupTimeoutMs = _hostLookupTimeoutMs,
            DeactivatedClientsExpiry = _deactivatedClientsExpiry,
            DisableVerboseLogging = _disableVerboseLogging,
            BackoffMultiplier = _backoffMultiplier,
            MaxPoolSize = _maxPoolSize,
            MessageTimeToLive = _messageTimeToLive,
            RedisConnectionString = _redisConnectionString
        };
    }
}