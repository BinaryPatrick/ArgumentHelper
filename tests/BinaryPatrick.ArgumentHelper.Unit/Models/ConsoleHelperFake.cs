using System;
using System.Collections.Generic;
using BinaryPatrick.ArgumentHelper.Services;

namespace BinaryPatrick.ArgumentHelper.Unit.Models;
internal class ConsoleHelperFake : ConsoleHelper
{
    public List<string> Output { get; } = new List<string>();

    public override void WriteText(string text, ConsoleColor? foregroundColor = null)
        => Output.Add(text);
}
