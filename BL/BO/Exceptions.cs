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

public class BlIncorrectInputException : Exception
{
    public BlIncorrectInputException(string? message) : base(message) { }
}

public class BlInvalidDatesException : Exception  
{
    public BlInvalidDatesException(string? message) : base(message) { }
}
public class BlcanNotBeDeletedException : Exception
{
    public BlcanNotBeDeletedException(string? message) : base(message) { }
}