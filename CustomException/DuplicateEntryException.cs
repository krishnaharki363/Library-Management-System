using System;

namespace LibraryManagementSystem.CustomException
{
    /// <summary>
    /// Custom exception for duplicate entries
    /// </summary>
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message) { }
    }
}
