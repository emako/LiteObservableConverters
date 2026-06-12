using System.Windows.Markup;

namespace LiteObservableConverters;

/// <inheritdoc/>
[MarkupExtensionReturnType(typeof(object))]
public class UnDynamicResourceExtension(object? resourceKey) : StaticResourceExtension(resourceKey);
