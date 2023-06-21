using BinaryPatrick.ArgumentHelper.Exceptions;
using BinaryPatrick.ArgumentHelper.Extensions;
using BinaryPatrick.ArgumentHelper.Interfaces;
using BinaryPatrick.ArgumentHelper.Models;

namespace BinaryPatrick.ArgumentHelper.Services;

internal class ArgumentParser<T> : IArgumentParser<T> where T : class, new()
{
    private static readonly List<RequiredProperty> requiredProperties = GetRequiredProperties();
    private static readonly List<OptionalProperty> optionalProperties = GetOptionalProperties();

    private readonly IConsoleHelper consoleHelper;

    public string? Description { get; set; }

    public ArgumentParser(IConsoleHelper consoleHelper)
    {
        this.consoleHelper = consoleHelper;
    }

    public (string FullFlag, string ShortFlag) HelpFlags { get; set; } = ("help", "?");

    public IArgumentParser<T> SetHelpFlags(string fullFlag, string shortFlag)
    {
        HelpFlags = (fullFlag, shortFlag);
        return this;
    }

    public IArgumentParser<T> AddDescription(string description)
    {
        Description = description;
        return this;
    }

    public IArgumentParser<T> ShowHelpText()
    {
        consoleHelper.WriteHelpText<T>(Description, requiredProperties, optionalProperties);
        return this;
    }

    public T Parse(string[] arguments)
    {
        ExitIfHelpFlagRaised(arguments);
        ExitIfTooFewArguments(arguments);

        T options = new T();
        TrySetRequiredProperties(options, arguments[..requiredProperties.Count]);
        TrySetOptionalProperties(options, arguments[requiredProperties.Count..]);

        return options;
    }

    public static List<RequiredProperty> GetRequiredProperties()
        => typeof(T).GetProperties()
            .Select(x => x.GetRequiredArgumentProperty())
            .Where(x => x is not null)
            .OrderBy(x => x!.ArgumentAttribute.Order)
            .ToList()!;

    public static List<OptionalProperty> GetOptionalProperties()
        => typeof(T).GetProperties()
            .Select(x => x.GetOptionalArgumentProperty())
            .Where(x => x is not null)
            .OrderBy(x => x!.ArgumentAttribute.FullName)
            .ToList()!;

    public void TrySetRequiredProperties(T obj, string[] arguments)
    {
        foreach ((RequiredProperty property, string value) in requiredProperties.Zip(arguments))
        {
            bool isSet = property.TrySetValue(obj, value!);
            if (!isSet)
            {
                ExitWithParsingError($"Invalid argument value given. '{value}' is not a valid argument for '{property!.ArgumentAttribute.FullName}'.");
            }
        }
    }

    public void TrySetOptionalProperties(T obj, string[] arguments)
    {
        IEnumerable<ArgumentPair> pairs = GetPairs(arguments);
        foreach (ArgumentPair pair in pairs)
        {
            OptionalProperty? property = optionalProperties.FirstOrDefault(x => x.ArgumentAttribute.HasMatchingFlag(pair.Flag));
            if (property is null)
            {
                ExitWithUnknownArgument($"Unknown argument {pair.Flag}");
            }

            bool isSet = property!.TrySetValue(obj, pair.Value!);
            if (!isSet)
            {
                ExitWithParsingError($"Invalid argument value given. '{pair.Value}' is not a valid argument for '{property!.ArgumentAttribute.FullName}'.");
            }
        }
    }

    public void ExitIfHelpFlagRaised(string[] arguments)
    {
        if (!arguments.Contains($"--{HelpFlags.FullFlag}") && !arguments.Contains($"-{HelpFlags.ShortFlag}"))
        {
            return;
        }

        consoleHelper.WriteHelpText<T>(Description, requiredProperties, optionalProperties);
        throw new HelpFlagException("Help flag argument given");
    }

    public void ExitIfTooFewArguments(string[] arguments)
    {
        if (arguments.Length >= requiredProperties.Count)
        {
            return;
        }

        consoleHelper.WriteError("Not all required arguments provided");
        consoleHelper.WriteHelpText<T>(Description, requiredProperties, optionalProperties);
        throw new ArgumentException("Not all required arguments provided");
    }

    public void ExitWithParsingError(string error)
    {
        consoleHelper.WriteError(error);
        consoleHelper.WriteHelpText<T>(Description, requiredProperties, optionalProperties);
        throw new ArgumentParsingException(error);
    }

    public void ExitWithUnknownArgument(string error)
    {
        consoleHelper.WriteError(error);
        consoleHelper.WriteHelpText<T>(Description, requiredProperties, optionalProperties);
        throw new UnknownArgumentException(error);
    }

    public List<ArgumentPair> GetPairs(string[] arguments)
    {
        List<ArgumentPair> pairs = new List<ArgumentPair>();
        ArgumentPair? current = null;
        for (int i = 0; i < arguments.Length; i++)
        {
            string value = arguments[i];
            if (value.StartsWith('-'))
            {
                current = new ArgumentPair(value);
                pairs.Add(current);
                continue;
            }

            if (current is null)
            {
                consoleHelper.WriteError($"Unknown argument {value}");
                continue;
            }

            current.Value = value;
        }

        return pairs;
    }
}
