using System.Reflection;

namespace BinaryPatrick.ArgumentHelper.Models;


internal class RequiredProperty : BaseProperty<RequiredArgumentAttribute>
{
    public RequiredProperty(PropertyInfo propertyInfo, RequiredArgumentAttribute argumentAttribute) : base(propertyInfo, argumentAttribute) { }

    internal string GetHelpText()
        => $"{ArgumentAttribute.FullName.ToUpper()} {ArgumentAttribute.Description} [required]\r\n";
}
