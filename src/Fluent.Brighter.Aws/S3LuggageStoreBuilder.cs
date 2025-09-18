using System.Collections.Generic;
using System.Net.Http;

using Amazon.S3;
using Amazon.S3.Model;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;
using Paramore.Brighter.Tranformers.AWS;
using Paramore.Brighter.Transforms.Storage;

using Polly.Retry;

namespace Fluent.Brighter.Aws;

public sealed class S3LuggageStoreBuilder
{
    private AWSS3Connection? _connection;
    public S3LuggageStoreBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = new AWSS3Connection(connection.Credentials, connection.Region, connection.ClientConfigAction);
        if (_s3Region == null)
        {
            _s3Region = new S3Region(connection.Region.SystemName);
        }
        
        return this;
    }
    
    private string? _bucketName;
    public S3LuggageStoreBuilder SetBucketName(string bucketName)
    {
        _bucketName = bucketName;
        return this;
    }
    
    private S3CannedACL? _acls;
    public S3LuggageStoreBuilder SetACLs(S3CannedACL acls)
    {
        _acls = acls;
        return this;
    }
    
    
    private string _luggagePrefix = "BRIGHTER_CHECKED_LUGGAGE";
    public S3LuggageStoreBuilder SetLuggagePrefix(string luggagePrefix)
    {
        _luggagePrefix = luggagePrefix;
        return this;
    }
    
    private IHttpClientFactory? _httpClientFactory;
    public S3LuggageStoreBuilder SetHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        return this;
    }

    private List<Tag> _tags = [new() { Key = "Creator", Value = " Brighter Luggage Store" }];
    public S3LuggageStoreBuilder SetTags(List<Tag> tags)
    {
        _tags = tags;
        return this;
    }
    
    public S3LuggageStoreBuilder AddTag(Tag tag)
    {
        _tags.Add(tag);
        return this;
    }

    private int _timeToAbortFailedUploads  = 1;
    public S3LuggageStoreBuilder SetTimeToAbortFailedUploads(int days)
    {
        _timeToAbortFailedUploads = days;
        return this;
    }
    
    private int _timeToDeleteGoodUploads = 7;
    public S3LuggageStoreBuilder SetTimeToDeleteGoodUploads(int days)
    {
        _timeToAbortFailedUploads = days;
        return this;
    }
    
    private string _bucketAddressTemplate = S3LuggageOptions.DefaultBucketAddressTemplate;
    public S3LuggageStoreBuilder SetBucketAddressTemplate(string bucketAddressTemplate)
    {
        _bucketAddressTemplate = bucketAddressTemplate;
        return this;
    }
    
    private AsyncRetryPolicy? _retryPolicy;
    public S3LuggageStoreBuilder SetRetryPolicy(AsyncRetryPolicy? retryPolicy)
    {
        _retryPolicy = retryPolicy;
        return this;
    }
    
    private StorageStrategy _strategy = StorageStrategy.Assume;
    public S3LuggageStoreBuilder SetStrategy(StorageStrategy strategy)
    {
        _strategy = strategy;
        return this;
    }
    
    private S3Region? _s3Region;
    public S3LuggageStoreBuilder SetRegion(S3Region s3Region)
    {
        _s3Region = s3Region;
        return this;
    }
    
    internal S3LuggageStore Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is null");
        }

        if (string.IsNullOrEmpty(_bucketName))
        {
            throw new ConfigurationException("Bucket name is null or empty");
        }

        return new S3LuggageStore(new S3LuggageOptions(_connection, _bucketName)
        {
            ACLs = _acls,
            LuggagePrefix = _luggagePrefix,
            HttpClientFactory = _httpClientFactory,
            Tags = _tags,
            TimeToAbortFailedUploads = _timeToAbortFailedUploads,
            TimeToDeleteGoodUploads = _timeToDeleteGoodUploads,
            BucketAddressTemplate = _bucketAddressTemplate,
            RetryPolicy = _retryPolicy,
            Strategy = _strategy,
            BucketRegion = _s3Region!,
        });
    }
}