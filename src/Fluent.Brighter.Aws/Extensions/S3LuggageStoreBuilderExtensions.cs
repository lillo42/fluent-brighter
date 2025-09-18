using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.S3.Model;

using Fluent.Brighter.Aws;

using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

public static class S3LuggageStoreBuilderExtensions
{
    public static S3LuggageStoreBuilder SetConnection(this S3LuggageStoreBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
    
    public static S3LuggageStoreBuilder SetTags(this S3LuggageStoreBuilder builder, IEnumerable<Tag> tags)
        => builder.SetTags(tags.ToList());
    
    public static S3LuggageStoreBuilder SetTags(this S3LuggageStoreBuilder builder, params Tag[] tags)
        => builder.SetTags(tags.ToList());
    
    public static S3LuggageStoreBuilder AddTag(this S3LuggageStoreBuilder builder, string key, string value) 
        => builder.AddTag(new Tag{ Key = key, Value = value});
    
    
    public static S3LuggageStoreBuilder CreateIfMissing(this S3LuggageStoreBuilder builder) 
        => builder.SetStrategy(StorageStrategy.CreateIfMissing);
    
    public static S3LuggageStoreBuilder ValidIfS3Exists(this S3LuggageStoreBuilder builder) 
        => builder.SetStrategy(StorageStrategy.Validate);
    
    public static S3LuggageStoreBuilder AssumeS3Exists(this S3LuggageStoreBuilder builder) 
        => builder.SetStrategy(StorageStrategy.Assume);
}