using Fluent.Brighter.Kafka;

using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="KafkaMessagingGatewayConfigurationBuilder"/>
/// to simplify configuration of common SASL mechanisms and security protocols
/// when integrating Brighter with Apache Kafka.
/// </summary>
public static class KafkaMessagingGatewayConfigurationBuilderExtensions
{
    #region SaslMechanism

    /// <summary>
    /// Configures the Kafka client to use GSSAPI (Kerberos) SASL authentication.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseGssapiSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSaslMechanisms(SaslMechanism.Gssapi);
    }

    /// <summary>
    /// Configures the Kafka client to use PLAIN SASL authentication (username/password).
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UsePlainSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSaslMechanisms(SaslMechanism.Plain);
    }

    /// <summary>
    /// Configures the Kafka client to use SCRAM-SHA-256 SASL authentication.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseScramSha256SaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSaslMechanisms(SaslMechanism.ScramSha256);
    }

    /// <summary>
    /// Configures the Kafka client to use SCRAM-SHA-512 SASL authentication.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseScramSha512SaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSaslMechanisms(SaslMechanism.ScramSha512);
    }

    /// <summary>
    /// Configures the Kafka client to use OAuthBearer SASL authentication.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseOAuthBearerSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSaslMechanisms(SaslMechanism.OAuthBearer);
    }

    #endregion
    
    #region SecurityProtocol

    /// <summary>
    /// Configures the Kafka client to communicate over an unencrypted plaintext connection.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UsePlaintextSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSecurityProtocol(SecurityProtocol.Plaintext);
    }

    /// <summary>
    /// Configures the Kafka client to use SSL/TLS encryption without SASL authentication.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseSslSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSecurityProtocol(SecurityProtocol.Ssl);
    }

    /// <summary>
    /// Configures the Kafka client to use SASL authentication over a plaintext connection (not recommended for production).
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseSaslPlaintextSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSecurityProtocol(SecurityProtocol.SaslPlaintext);
    }

    /// <summary>
    /// Configures the Kafka client to use SASL authentication over an SSL/TLS encrypted connection.
    /// </summary>
    /// <param name="builder">The configuration builder instance.</param>
    /// <returns>The builder for chaining.</returns>
    public static KafkaMessagingGatewayConfigurationBuilder UseSaslSslSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
    {
        return builder.SetSecurityProtocol(SecurityProtocol.SaslSsl);
    }

    #endregion
}