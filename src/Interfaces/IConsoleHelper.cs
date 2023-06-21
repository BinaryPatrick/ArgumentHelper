using BinaryPatrick.ArgumentHelper.Models;

namespace BinaryPatrick.ArgumentHelper.Interfaces;

internal interface IConsoleHelper
{
    void WriteHelpText<T>(string? description, IEnumerable<RequiredProperty> requiredProperties, IEnumerable<OptionalProperty> optionalProperties) where T : new();

    void WriteError(string error);
}