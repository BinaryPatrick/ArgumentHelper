# Argument Helper

This is a command line argument parser that is lightweight and easy to use. It allows the use of attributes to define command line arguments, and the enable conversion into a given type.

## Quickstart

```csharp
public class AppArguments
{
    [RequiredArgument("directory", "This represents a required string value", 0)]
    public string Directory { get; set; }

    [OptionalArgument("extension", "This represents an optional argument", ShortFlag = "e")]
    public string? FileExtension { get; set; }
}
```

```csharp
public class Program {
    public void Main(string[] args) {
        ArgumentParser<AppArguments> parser = ArgumentParser.Initialize<AppArguments>();
        AppArguments aa = parser.Parse(args);
    }
}
```

It has been tested to work with primatives and enums. It also produces a nicely formatted help output when it detects an error and includes logging.

## Automatically Generated Help

You can add custom help flags by using:
```
IArgumentParser<T>.SetHelpFlags(fullFlag, shortFlag)
````

You can also add a descriptions using:
```
IArgumentParser<T>.AddDescription(text)`
```

Example of automatically generated help message

```
Usage: YourApp DIRECTORY [OPTIONS]

Arguments:
    DIRECTORY This represents a required string value [required]

Options:
    --extension, -e         This represents an optional argument

```