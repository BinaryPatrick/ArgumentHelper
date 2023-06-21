using System.Reflection;
using BinaryPatrick.ArgumentHelper.Models;

namespace BinaryPatrick.ArgumentHelper.Extensions;

internal static class ArgumentExtensions
{
    public static bool TryParse(this Type type, string input, out object? @object)
    {
        try
        {
            @object = Convert.ChangeType(input, type);
            return true;
        }
        catch
        {
            @object = null;
            return false;
        }
    }

    public static RequiredProperty? GetRequiredArgumentProperty(this PropertyInfo propertyInfo)
    {
        RequiredArgumentAttribute? argumentAttribute = propertyInfo.GetCustomAttribute<RequiredArgumentAttribute>();
        if (argumentAttribute is null)
        {
            return null;
        }

        RequiredProperty requiredArgument = new RequiredProperty(propertyInfo, argumentAttribute);
        return requiredArgument;
    }

    public static OptionalProperty? GetOptionalArgumentProperty(this PropertyInfo propertyInfo)
    {
        OptionalArgumentAttribute? argumentAttribute = propertyInfo.GetCustomAttribute<OptionalArgumentAttribute>();
        if (argumentAttribute is null)
        {
            return null;
        }

        OptionalProperty optionalArgument = new OptionalProperty(propertyInfo, argumentAttribute);
        return optionalArgument;
    }

    public static bool IsNullOrEmpty(this string? value)
        => string.IsNullOrEmpty(value);

    public static bool HasValue(this string? value)
        => !string.IsNullOrEmpty(value);
}
