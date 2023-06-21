using System.Runtime.Serialization;

namespace BinaryPatrick.ArgumentHelper.Exceptions;
internal class HelpFlagException : Exception
{
    public HelpFlagException() { }

    public HelpFlagException(string? message) : base(message) { }

    public HelpFlagException(string? message, Exception? innerException) : base(message, innerException) { }

    protected HelpFlagException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
