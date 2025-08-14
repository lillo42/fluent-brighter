using System;

using Fluent.Brighter.RMQ.Async;

namespace Fluent.Brighter;

public static class AmqpUriSpecificationBuilderExtensions
{
    public static AmqpUriSpecificationBuilder SetUri(this AmqpUriSpecificationBuilder builder, string uri)
        => builder.SetUri(new Uri(uri, UriKind.RelativeOrAbsolute));
}