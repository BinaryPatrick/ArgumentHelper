using System.Reflection;
using BinaryPatrick.ArgumentHelper.Attributes;
using BinaryPatrick.ArgumentHelper.Extensions;

namespace BinaryPatrick.ArgumentHelper.Models;

internal abstract class BaseProperty<T> where T : ArgumentAttribute
{
    public BaseProperty(PropertyInfo propertyInfo, T argumentAttribute)
    {
        PropertyInfo = propertyInfo;
        ArgumentAttribute = argumentAttribute;
    }

    public PropertyInfo PropertyInfo { get; init; }
    public T ArgumentAttribute { get; init; }

    public virtual bool TrySetValue(object obj, string value)
    {
        Type? propertyType = PropertyInfo.PropertyType;
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propertyType = Nullable.GetUnderlyingType(PropertyInfo.PropertyType);
        }

        if (propertyType is null)
        {
            return false;
        }

        // Parse Enum
        if (propertyType.IsEnum)
        {
            return SaveEnumIfValid(propertyType, obj, value);
        }

        // Parse Base Type
        return SaveValueIfValid(propertyType, obj, value);
    }

    protected virtual bool SaveValueIfValid(Type propertyType, object obj, string value)
    {
        if (!propertyType.TryParse(value, out object? parsedValue))
        {
            return false;
        }

        PropertyInfo.SetValue(obj, parsedValue);
        return true;
    }

    protected virtual bool SaveEnumIfValid(Type propertyType, object obj, string value)
    {
        if (!Enum.TryParse(propertyType, value, true, out object? enumValue))
        {
            return false;
        }

        PropertyInfo.SetValue(obj, enumValue);
        return true;
    }
}
