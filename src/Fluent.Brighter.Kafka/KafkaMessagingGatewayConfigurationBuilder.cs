using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaMessagingGatewayConfigurationBuilder
{
    private string[]? _bootStrapServers;

    public KafkaMessagingGatewayConfigurationBuilder SetBootstrapServers(string bootStrapServer)
    {
        _bootStrapServers = [bootStrapServer];
        return this;
    }

    public KafkaMessagingGatewayConfigurationBuilder SetBootstrapServers(params string[] bootStrapServers)
    {
        _bootStrapServers = bootStrapServers;
        return this;
    }

    private string? _debug = null;
    
    public KafkaMessagingGatewayConfigurationBuilder SetDebug(string debug)
    {
        _debug = debug;
        return this;
    }

    private string? _name = null;

    public KafkaMessagingGatewayConfigurationBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private SaslMechanism? _saslMechanisms = null;

    public KafkaMessagingGatewayConfigurationBuilder SetSaslMechanisms(SaslMechanism saslMechanisms)
    {
        _saslMechanisms = saslMechanisms;
        return this;
    }

    private string? _saslKerberosPrincipal = null;

    public KafkaMessagingGatewayConfigurationBuilder SetSaslKerberosPrincipal(string saslKerberosPrincipal)
    {
        _saslKerberosPrincipal = saslKerberosPrincipal;
        return this;
    }

    private string? _saslUsername;
    
    public KafkaMessagingGatewayConfigurationBuilder SetSaslUsername(string saslUsername)
    {
        _saslUsername = saslUsername;
        return this;
    }

    private string? _saslPassword;
    
    public KafkaMessagingGatewayConfigurationBuilder SetSaslPassword(string saslPassword)
    {
        _saslPassword = saslPassword;
        return this;
    }

    private SecurityProtocol? _securityProtocol = null;
    
    public KafkaMessagingGatewayConfigurationBuilder SetSecurityProtocol(SecurityProtocol securityProtocol)
    {
        _securityProtocol = securityProtocol;
        return this;
    }

    private string? _sslCaLocation = null;

    public KafkaMessagingGatewayConfigurationBuilder SetSslCaLocation(string sslCaLocation)
    {
        _sslCaLocation = sslCaLocation;
        return this;
    }

    private string? _sslKeystoreLocation = null;
    public KafkaMessagingGatewayConfigurationBuilder SetSslKeystoreLocation(string sslKeystoreLocation)
    {
        _sslKeystoreLocation = sslKeystoreLocation;
        return this;
    }

    
    private string? _sslKeystorePassword = null;

    public KafkaMessagingGatewayConfigurationBuilder SetSslKeystorePassword(string sslKeystorePassword)
    {
        _sslKeystorePassword = sslKeystorePassword;
        return this;
    }

    /// <summary>
    /// Builds a new instance of KafkaMessagingGatewayConfiguration.
    /// </summary>
    public KafkaMessagingGatewayConfiguration Build()
    {
        if (_bootStrapServers == null || _bootStrapServers.Length == 0)
        {
            throw new ConfigurationException("bootStrapServers was not est");
        }
        return new KafkaMessagingGatewayConfiguration
        {
            BootStrapServers = _bootStrapServers,
            Debug = _debug,
            Name = _name,
            SaslMechanisms = _saslMechanisms,
            SaslKerberosPrincipal = _saslKerberosPrincipal,
            SaslUsername = _saslUsername,
            SaslPassword = _saslPassword,
            SecurityProtocol = _securityProtocol,
            SslCaLocation = _sslCaLocation,
            SslKeystoreLocation = _sslKeystoreLocation,
            SslKeystorePassword = _sslKeystorePassword
        };
    }
}