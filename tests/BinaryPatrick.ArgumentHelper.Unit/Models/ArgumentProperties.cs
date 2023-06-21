namespace BinaryPatrick.ArgumentHelper.Unit.Models;

internal class ArgumentProperties
{
    [RequiredArgument("required", "This is a test string value", 0)]
    public string? RequiredString { get; set; }

    [OptionalArgument("test-string", "This is a test string value", ShortFlag = "ts")]
    public string? TestString { get; set; }

    [OptionalArgument("test-string-with-default", "This is a test string value with default")]
    public string TestStringWithDefault { get; set; } = "default test value";

    [OptionalArgument("test-int", "This is a test int value", ShortFlag = "ti")]
    public int? TestInt { get; set; }

    [OptionalArgument("test-int-with-default", "This is a test int value with default")]
    public int TestIntWithDefault { get; set; } = 42;

    [OptionalArgument("test-double", "This is a test double value", ShortFlag = "td")]
    public double? TestDouble { get; set; }

    [OptionalArgument("test-double-with-default", "This is a test double value with default")]
    public double TestDoubleWithDefault { get; set; } = 4.2;

    [OptionalArgument("test-enum", "This is a test enum value", ShortFlag = "te")]
    public TestEnum? TestEnums { get; set; }

    [OptionalArgument("test-enum-with-default", "This is a test enum value with default")]
    public TestEnum TestEnumsWithDefault { get; set; } = TestEnum.One;
}

public class AppArguments
{
    [RequiredArgument("directory", "This represents a required string value", 0)]
    public string Directory { get; set; }

    [OptionalArgument("extension", "This represents an optional argument", ShortFlag = "e")]
    public string? FileExtension { get; set; }
}