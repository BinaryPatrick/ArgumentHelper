using System.Runtime.Serialization;

namespace BinaryPatrick.ArgumentHelper.Exceptions;

internal class ArgumentParsingException : Exception
{
    public ArgumentParsingException() { }

    public ArgumentParsingException(string? message) : base(message) { }

    public ArgumentParsingException(string? message, Exception? innerException) : base(message, innerException) { }

    protected ArgumentParsingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
