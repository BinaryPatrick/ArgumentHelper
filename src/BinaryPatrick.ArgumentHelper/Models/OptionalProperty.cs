using System.Reflection;
using System.Text;
using BinaryPatrick.ArgumentHelper.Extensions;

namespace BinaryPatrick.ArgumentHelper.Models;

internal class OptionalProperty : BaseProperty<OptionalArgumentAttribute>
{
    public OptionalProperty(PropertyInfo propertyInfo, OptionalArgumentAttribute argumentAttribute) : base(propertyInfo, argumentAttribute) { }

    public override bool TrySetValue(object obj, string value)
    {
        if (PropertyInfo.PropertyType == typeof(bool) && value is null)
        {
            PropertyInfo.SetValue(obj, true);
            return true;
        }

        return base.TrySetValue(obj, value);
    }

    internal string GetHelpText()
    {
        StringBuilder flagsBuilder = new StringBuilder();
        flagsBuilder.Append($"--{ArgumentAttribute.FullName}");
        if (ArgumentAttribute.ShortFlag.HasValue())
        {
            flagsBuilder.Append($", -{ArgumentAttribute.ShortFlag} ");
        }
        string flags = flagsBuilder.ToString().PadRight(20);
        return $"{flags}    {ArgumentAttribute.Description}\r\n";
    }

    internal string GetDefaultValue(object obj)
    {
        object? defaultValue = PropertyInfo.GetValue(obj);
        if (defaultValue is null)
        {
            return string.Empty;
        }

        return $"[Default: {defaultValue}]\r\n";
    }
}
