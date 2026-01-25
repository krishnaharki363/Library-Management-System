using System;

namespace LibraryManagementSystem.CustomException
{
    /// <summary>
    /// Custom exception for invalid item data
    /// </summary>
    public class InvalidItemDataException : Exception
    {
        public InvalidItemDataException(string message) : base(message) { }
        
        public InvalidItemDataException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
