using System;

namespace LiteObservableConverters;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class LocaleDescriptionAttribute(string locale, string description, bool isFallback = false) : Attribute
{
    private readonly string locale = locale;
    public virtual string Locale => locale;

    private readonly string description = description;
    public virtual string Description => description;

    private readonly bool isFallback = isFallback;
    public virtual bool IsFallback => isFallback;

    public LocaleDescriptionAttribute() : this(string.Empty, string.Empty)
    {
    }

    public LocaleDescriptionAttribute(string description) : this(string.Empty, description)
    {
    }
}
