using System.Diagnostics;
using System.Text;
using BinaryPatrick.ArgumentHelper.Extensions;
using BinaryPatrick.ArgumentHelper.Interfaces;
using BinaryPatrick.ArgumentHelper.Models;

namespace BinaryPatrick.ArgumentHelper.Services;

internal class ConsoleHelper : IConsoleHelper
{
    public virtual void WriteError(string error)
        => WriteText(error, ConsoleColor.Red);

    public virtual void WriteHelpText<T>(IEnumerable<RequiredProperty> requiredProperties, IEnumerable<OptionalProperty> optionalProperties) where T : new()
    {
        StringBuilder text = CreateHeadline(requiredProperties);

        if (requiredProperties.Any())
        {
            StringBuilder requiredArguments = CreateRequiredArguments(requiredProperties);
            text.Append(requiredArguments);
        }

        if (optionalProperties.Any())
        {
            StringBuilder optionalArguments = CreateOptionalArguments<T>(optionalProperties);
            text.Append(optionalArguments);
        }

        WriteText(text.ToString());
    }

    public virtual StringBuilder CreateHeadline(IEnumerable<RequiredProperty> properties)
    {
        IEnumerable<string> argumentNames = properties.Select(x => x.ArgumentAttribute.FullName.ToUpper());
        string argumentNamesStr = string.Join(' ', argumentNames);
        string processName = Process.GetCurrentProcess().ProcessName;

        StringBuilder headline = new StringBuilder();
        headline.AppendLine($"Usage: {processName} {argumentNamesStr} [OPTIONS]");
        headline.AppendLine("    Prune lets you programmatically remove older archives and retain archives\r\n" +
                            "    for the last given number of time intervals. The retention options are\r\n" +
                            "    processed in the order given above. Each option only covers backups within\r\n" +
                            "    its time period. The next option does not take care of already covered\r\n" +
                            "    backups. It will only consider older backups.Note: ISO week date are in use\r\n" +
                            "    and the week begins on Monday.\r\n");
        return headline;
    }

    public virtual StringBuilder CreateRequiredArguments(IEnumerable<RequiredProperty> properties)
    {
        string requiredArguments = properties
            .Aggregate(new StringBuilder(), (a, b) =>
            {
                a.Append("    " + b.GetHelpText());
                return a;
            })
            .ToString();

        StringBuilder arguments = new StringBuilder();
        arguments.AppendLine("Arguments:");
        arguments.AppendLine(requiredArguments);

        return arguments;
    }

    public virtual StringBuilder CreateOptionalArguments<T>(IEnumerable<OptionalProperty> properties) where T : new()
    {
        T obj = new T();
        StringBuilder optionalArguments = properties
            .Aggregate(new StringBuilder(), (a, b) =>
            {
                a.Append($"    {b.GetHelpText()}");

                string defaultValue = b.GetDefaultValue(obj);
                if (defaultValue.IsNullOrEmpty())
                {
                    return a;
                }

                a.Append($"        {defaultValue}");
                return a;
            });

        StringBuilder arguments = new StringBuilder();
        arguments.AppendLine("Options:");
        arguments.Append(optionalArguments);

        return arguments;
    }

    public virtual void WriteText(string text, ConsoleColor? foregroundColor = null)
    {
        if (foregroundColor is null)
        {
            Console.WriteLine(text);
            return;
        }

        ConsoleColor initialColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor.Value;
        Console.WriteLine(text);
        Console.ForegroundColor = initialColor;

    }
}
