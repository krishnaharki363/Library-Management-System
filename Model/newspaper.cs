using System;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem.Model
{
    /// <summary>
    /// Newspaper class derived from LibraryItemBase
    /// </summary>
    public class Newspaper : LibraryItemBase
    {
        // Private field with proper naming convention
        private string _edition = null!;

        // Public property for Edition with validation
        public string Edition
        {
            get { return _edition; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Edition cannot be null or empty.");

                // 2. Check for minimum length (at least 3 characters)
                if (value.Length < 3)
                    throw new InvalidItemDataException("Edition must be at least 3 characters long.");

                // 3. Check if it begins with a capital letter
                if (!char.IsUpper(value[0]))
                    throw new InvalidItemDataException("Edition must begin with a capital letter.");

                _edition = value;
            }
        }

        // Constructor
        public Newspaper(string title, string publisher, string publicationYear, string edition)
            : base(title, publisher, publicationYear)
        {
            Edition = edition;
        }

        // Override DisplayInfo to show Newspaper-specific information
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Edition: {Edition}");
            Console.WriteLine($"Type: Newspaper");
            Console.WriteLine("-----------------------------------");
        }
    }
}