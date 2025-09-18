using System;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter.Tranformers.AWS;
using Paramore.Brighter.Tranformers.AWS.V4;

namespace Fluent.Brighter;

public static class LuggageStoreBuilderExtensions
{
    public static LuggageStoreBuilder UseS3LuggageStore(this LuggageStoreBuilder builder,
        Action<S3LuggageStoreBuilder> configure)
    {
        var store = new S3LuggageStoreBuilder();
        configure(store);
        return builder.UseS3LuggageStore(store.Build());
    }
    
    public static LuggageStoreBuilder UseS3LuggageStore(this LuggageStoreBuilder builder, S3LuggageStore store)
    {
        builder.EnableLuggageStore(store);
        return builder;
    }
}