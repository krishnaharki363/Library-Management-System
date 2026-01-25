using System;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem.Model
{
    /// <summary>
    /// Book class derived from Item
    /// </summary>
    public class Book : Item
    {
        // Private field with proper naming convention
        private string _author = null!;

        // Public property for Author with validation
        public string Author
        {
            get { return _author; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Author cannot be null or empty.");
                
                // 2. Check for minimum length (at least 5 characters)
                if (value.Length < 5)
                    throw new InvalidItemDataException("Author name must be at least 5 characters long.");
                
                // 3. Check if it begins with a capital letter
                if (!char.IsUpper(value[0]))
                    throw new InvalidItemDataException("Author name must begin with a capital letter.");
                
                _author = value;
            }
        }

        // Constructor
        public Book(string title, string publisher, string publicationYear, string author)
            : base(title, publisher, publicationYear)
        {
            Author = author;
        }

        // Override DisplayInfo to show Book-specific information
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Type: Book");
            Console.WriteLine("-----------------------------------");
        }
    }
}