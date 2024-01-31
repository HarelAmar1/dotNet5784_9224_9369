namespace BO
{
 
    [Serializable]
    public class BLDoesNotExistException : Exception
    {
        public BLDoesNotExistException(string? message) : base(message) { }
    }
    [Serializable]
    public class BLAlreadyExistsException : Exception
    {
        public BLAlreadyExistsException(string? message) : base(message) { }
    }
    [Serializable]
    public class BLDeletionImpossible : Exception
    {
        public BLDeletionImpossible(string? message) : base(message) { }
    }
    [Serializable]
    public class BLCanNotBeNULL : Exception
    {
        public BLCanNotBeNULL(string? message) : base(message) { }
    }

    public class BLXMLFileLoadCreateException : Exception
    {
        public BLXMLFileLoadCreateException(string? message) : base(message) { }
    }

}
}
