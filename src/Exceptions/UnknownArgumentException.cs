using System.Runtime.Serialization;

namespace BinaryPatrick.ArgumentHelper.Exceptions;

internal class UnknownArgumentException : Exception
{
    public UnknownArgumentException() { }

    public UnknownArgumentException(string? message) : base(message) { }

    public UnknownArgumentException(string? message, Exception? innerException) : base(message, innerException) { }

    protected UnknownArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
