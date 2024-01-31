namespace BO;


[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException): base(message, innerException) { }
}
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
}
[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
    public BlDeletionImpossible(string message, Exception innerException) : base(message, innerException) { }
}
[Serializable]
public class BlCanNotBeNULL : Exception
{
    public BlCanNotBeNULL(string? message) : base(message) { }
    public BlCanNotBeNULL(string message, Exception innerException) : base(message, innerException) { }
}

public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException) { }
}
public class BlTheInputIsIncorrect : Exception
{
    public BlTheInputIsIncorrect(string? message) : base(message) { }
    public BlTheInputIsIncorrect(string message, Exception innerException) : base(message, innerException) { }
}
