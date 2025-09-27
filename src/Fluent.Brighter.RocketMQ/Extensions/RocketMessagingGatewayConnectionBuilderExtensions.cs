using System;

using Fluent.Brighter.RocketMQ;

using Org.Apache.Rocketmq;

namespace Fluent.Brighter;

/// <summary>
/// 
/// </summary>
public static class RocketMessagingGatewayConnectionBuilderExtensions
{
    /// <summary>
    /// Configures the RocketMQ client using a fluent builder pattern for ClientConfig.
    /// Allows detailed configuration of endpoints, credentials, and client settings.
    /// </summary>
    /// <param name="connectionBuilder">The connection builder instance</param>
    /// <param name="configure">Action to configure the ClientConfig builder</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public static RocketMessagingGatewayConnectionBuilder SetClient(
        this RocketMessagingGatewayConnectionBuilder connectionBuilder, Action<ClientConfig.Builder> configure)
    {
        var builder = new ClientConfig.Builder();
        configure(builder);
        connectionBuilder.SetClient(builder.Build());
        return connectionBuilder;
    }
    
    /// <summary>
    /// Configures the RocketMQ client with a simple endpoint string.
    /// Convenience method for basic endpoint configuration.
    /// </summary>
    /// <param name="connectionBuilder">The connection builder instance</param>
    /// <param name="endpoint">The RocketMQ service endpoint URL</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public static RocketMessagingGatewayConnectionBuilder SetClient(
        this RocketMessagingGatewayConnectionBuilder connectionBuilder, string endpoint)
    {
        return connectionBuilder.SetClient(c => c.SetEndpoints(endpoint));
    }
}