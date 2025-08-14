using Fluent.Brighter.Kafka;

using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter;

public static class KafkaMessagingGatewayConfigurationBuilderExtensions
{
    #region SaslMechanism
    public static KafkaMessagingGatewayConfigurationBuilder UseGssapiSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSaslMechanisms(SaslMechanism.Gssapi);
    
    public static KafkaMessagingGatewayConfigurationBuilder UsePlainSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSaslMechanisms(SaslMechanism.Plain);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseScramSha256SaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSaslMechanisms(SaslMechanism.ScramSha256);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseScramSha512SaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSaslMechanisms(SaslMechanism.ScramSha512);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseOAuthBearerSaslMechanism(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSaslMechanisms(SaslMechanism.OAuthBearer);
    #endregion
    
    
    #region SecurityProtocol
    public static KafkaMessagingGatewayConfigurationBuilder UsePlaintextSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSecurityProtocol(SecurityProtocol.Plaintext);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseSslSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSecurityProtocol(SecurityProtocol.Ssl);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseSaslPlaintextSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSecurityProtocol(SecurityProtocol.SaslPlaintext);
    
    public static KafkaMessagingGatewayConfigurationBuilder UseSaslSslSecurityProtocol(
        this KafkaMessagingGatewayConfigurationBuilder builder)
        => builder.SetSecurityProtocol(SecurityProtocol.SaslSsl);
    #endregion
}