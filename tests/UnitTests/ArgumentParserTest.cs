using System;
using BinaryPatrick.ArgumentHelper.Exceptions;
using BinaryPatrick.ArgumentHelper.Services;
using BinaryPatrick.ArgumentHelper.Unit.Models;
using Xunit;

namespace BinaryPatrick.ArgumentHelper.Unit.UnitTests;

public class ArgumentParserTest
{
    [Fact]
    public void Parse_ShouldSucceed()
    {
        // Arrange
        string[] args = new string[]
        {
            "required",
            "--test-int", "3",
            "--test-double", "3.3",
            "--test-string", "test value",
            "--test-enum", TestEnum.Two.ToString(),
        };
        ConsoleHelperFake consoleFake = new ConsoleHelperFake();

        // Act
        IArgumentParser<ArgumentProperties> parser = new ArgumentParser<ArgumentProperties>(consoleFake);
        ArgumentProperties parserTest = parser.Parse(args);

        // Assert
        Assert.True(parser is not null);
        Assert.True(parserTest is not null);
        Assert.Equal("test value", parserTest!.TestString);
        Assert.Equal(3, parserTest.TestInt);
        Assert.Equal(3.3, parserTest.TestDouble);
        Assert.Equal(TestEnum.Two, parserTest.TestEnums);
        Assert.Empty(consoleFake.Output);
    }

    [Fact]
    public void Parse_ShouldSucceedWithOverrideDefaults()
    {
        // Arrange
        string[] args = new string[]
        {
            "required",
            "--test-int-with-default", "3",
            "--test-double-with-default", "3.3",
            "--test-string-with-default", "test value",
            "--test-enum-with-default", TestEnum.Two.ToString(),
        };
        ConsoleHelperFake consoleFake = new ConsoleHelperFake();

        // Act
        IArgumentParser<ArgumentProperties> parser = new ArgumentParser<ArgumentProperties>(consoleFake);
        ArgumentProperties parserTest = parser.Parse(args);

        // Assert
        Assert.True(parser is not null);
        Assert.True(parserTest is not null);
        Assert.Equal("test value", parserTest!.TestStringWithDefault);
        Assert.Equal(3, parserTest.TestIntWithDefault);
        Assert.Equal(3.3, parserTest.TestDoubleWithDefault);
        Assert.Equal(TestEnum.Two, parserTest.TestEnumsWithDefault);
        Assert.Empty(consoleFake.Output);
    }

    [Fact]
    public void Parse_ShouldFailWithMissingRequired()
    {
        // Arrange
        string[] args = Array.Empty<string>();
        ConsoleHelperFake consoleFake = new ConsoleHelperFake();
        IArgumentParser<ArgumentProperties> parser = new ArgumentParser<ArgumentProperties>(consoleFake);
        Exception? exception = null;

        // Act
        try
        {
            ArgumentProperties parserTest = parser.Parse(args);
        }
        catch (ArgumentException ex)
        {
            exception = ex;
        }

        // Assert
        Assert.True(parser is not null);
        Assert.True(exception is not null);

        bool isErrorOutputCorrect = consoleFake.Output.Count == 2;
        Assert.True(isErrorOutputCorrect);
    }

    [Fact]
    public void Parse_ShouldFailWithError()
    {
        // Arrange
        string[] args = new string[]
        {
            "required value",
            "--test", "3",
        };
        ConsoleHelperFake consoleFake = new ConsoleHelperFake();
        IArgumentParser<ArgumentProperties> parser = new ArgumentParser<ArgumentProperties>(consoleFake);
        Exception? exception = null;

        // Act
        try
        {
            ArgumentProperties parserTest = parser.Parse(args);
        }
        catch (UnknownArgumentException ex)
        {
            exception = ex;
        }

        // Assert
        Assert.True(parser is not null);
        Assert.True(exception is not null);

        bool isErrorOutputCorrect = consoleFake.Output.Count == 2;
        Assert.True(isErrorOutputCorrect);
    }

    [Theory]
    [InlineData("-?")]
    [InlineData("--help")]
    public void Parse_ShouldFailWithHelpFlag(string flag)
    {
        // Arrange
        string[] args = new string[] { flag };
        ConsoleHelperFake consoleFake = new ConsoleHelperFake();
        IArgumentParser<ArgumentProperties> parser = new ArgumentParser<ArgumentProperties>(consoleFake);
        Exception? exception = null;

        // Act
        try
        {
            ArgumentProperties parserTest = parser.Parse(args);
        }
        catch (HelpFlagException ex)
        {
            exception = ex;
        }

        // Assert
        Assert.True(parser is not null);
        Assert.True(exception is not null);

        bool isErrorOutputCorrect = consoleFake.Output.Count == 1;
        Assert.True(isErrorOutputCorrect);
    }
}