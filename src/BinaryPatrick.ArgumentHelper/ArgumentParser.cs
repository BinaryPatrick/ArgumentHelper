using System.Runtime.CompilerServices;
using BinaryPatrick.ArgumentHelper.Services;

[assembly: InternalsVisibleTo("BinaryPatrick.ArgumentHelper.Unit")]
namespace BinaryPatrick.ArgumentHelper;
public class ArgumentParser
{
    public static IArgumentParser<T> Initialize<T>() where T : class, new()
    {
        ConsoleHelper consoleHelper = new ConsoleHelper();
        ArgumentParser<T> parser = new ArgumentParser<T>(consoleHelper);
        return parser;
    }
}
