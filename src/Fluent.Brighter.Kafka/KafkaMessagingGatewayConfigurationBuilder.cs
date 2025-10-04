using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Provides a fluent builder for configuring Kafka messaging gateway settings
/// in a Brighter integration with Apache Kafka.
/// </summary>
public sealed class KafkaMessagingGatewayConfigurationBuilder
{
    private string[]? _bootStrapServers;

    /// <summary>
    /// Sets a single bootstrap server for the Kafka client.
    /// </summary>
    /// <param name="bootStrapServer">The Kafka bootstrap server address.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetBootstrapServers(string bootStrapServer)
    {
        _bootStrapServers = [bootStrapServer];
        return this;
    }

    /// <summary>
    /// Sets multiple bootstrap servers for the Kafka client.
    /// </summary>
    /// <param name="bootStrapServers">An array of Kafka bootstrap server addresses.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetBootstrapServers(params string[] bootStrapServers)
    {
        _bootStrapServers = bootStrapServers;
        return this;
    }

    private string? _debug = null;
    
    /// <summary>
    /// Configures the debug contexts for the underlying Kafka client (e.g., "broker,topic").
    /// </summary>
    /// <param name="debug">A comma-separated list of debug contexts.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetDebug(string debug)
    {
        _debug = debug;
        return this;
    }

    private string? _name = null;

    /// <summary>
    /// Sets a logical name for the Kafka client instance (used for logging and metrics).
    /// </summary>
    /// <param name="name">The client name.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private SaslMechanism? _saslMechanisms = null;

    /// <summary>
    /// Specifies the SASL mechanism to use for authentication.
    /// </summary>
    /// <param name="saslMechanisms">The SASL mechanism (e.g., PLAIN, SCRAM-SHA-256).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSaslMechanisms(SaslMechanism saslMechanisms)
    {
        _saslMechanisms = saslMechanisms;
        return this;
    }

    private string? _saslKerberosPrincipal = null;

    /// <summary>
    /// Sets the Kerberos principal name for SASL/GSSAPI authentication.
    /// </summary>
    /// <param name="saslKerberosPrincipal">The Kerberos principal.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSaslKerberosPrincipal(string saslKerberosPrincipal)
    {
        _saslKerberosPrincipal = saslKerberosPrincipal;
        return this;
    }

    private string? _saslUsername;
    
    /// <summary>
    /// Sets the username for SASL authentication (e.g., for PLAIN or SCRAM mechanisms).
    /// </summary>
    /// <param name="saslUsername">The SASL username.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSaslUsername(string saslUsername)
    {
        _saslUsername = saslUsername;
        return this;
    }

    private string? _saslPassword;
    
    /// <summary>
    /// Sets the password for SASL authentication.
    /// </summary>
    /// <param name="saslPassword">The SASL password.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSaslPassword(string saslPassword)
    {
        _saslPassword = saslPassword;
        return this;
    }

    private SecurityProtocol? _securityProtocol = null;
    
    /// <summary>
    /// Configures the security protocol used to communicate with Kafka brokers.
    /// </summary>
    /// <param name="securityProtocol">The security protocol (e.g., Plaintext, Ssl, SaslSsl).</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSecurityProtocol(SecurityProtocol securityProtocol)
    {
        _securityProtocol = securityProtocol;
        return this;
    }

    private string? _sslCaLocation = null;

    /// <summary>
    /// Sets the path to the CA certificate file for SSL verification.
    /// </summary>
    /// <param name="sslCaLocation">The file path to the CA certificate.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSslCaLocation(string sslCaLocation)
    {
        _sslCaLocation = sslCaLocation;
        return this;
    }

    private string? _sslKeystoreLocation = null;

    /// <summary>
    /// Sets the path to the SSL keystore file (e.g., PKCS#12 file).
    /// </summary>
    /// <param name="sslKeystoreLocation">The file path to the keystore.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSslKeystoreLocation(string sslKeystoreLocation)
    {
        _sslKeystoreLocation = sslKeystoreLocation;
        return this;
    }

    
    private string? _sslKeystorePassword = null;

    /// <summary>
    /// Sets the password for the SSL keystore.
    /// </summary>
    /// <param name="sslKeystorePassword">The keystore password.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessagingGatewayConfigurationBuilder SetSslKeystorePassword(string sslKeystorePassword)
    {
        _sslKeystorePassword = sslKeystorePassword;
        return this;
    }

    /// <summary>
    /// Builds a new instance of KafkaMessagingGatewayConfiguration.
    /// </summary>
    /// <returns>A fully configured <see cref="KafkaMessagingGatewayConfiguration"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown if bootstrap servers are not set.</exception>
    public KafkaMessagingGatewayConfiguration Build()
    {
        if (_bootStrapServers == null || _bootStrapServers.Length == 0)
        {
            throw new ConfigurationException("bootStrapServers was not set");
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