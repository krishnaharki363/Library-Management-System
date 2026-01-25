using System;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem.Model
{
    /// <summary>
    /// Abstract base class representing a library item
    /// </summary>
    public abstract class Item
    {
        // Private fields with proper naming convention
        private string _title = null!;
        private string _publisher = null!;
        private string _publicationYear = null!;

        // Public property for Title with validation
        public string Title
        {
            get { return _title; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Title cannot be null or empty.");
                
                // 2. Check for length (minimum 5 characters)
                if (value.Length < 5)
                    throw new InvalidItemDataException("Title must be at least 5 characters long.");
                
                // 3. Check if it begins with a capital letter
                if (!char.IsUpper(value[0]))
                    throw new InvalidItemDataException("Title must begin with a capital letter.");
                
                _title = value;
            }
        }

        // Public property for Publisher with validation
        public string Publisher
        {
            get { return _publisher; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Publisher cannot be null or empty.");
                
                // 2. Check for length (minimum 6 characters)
                if (value.Length < 6)
                    throw new InvalidItemDataException("Publisher must be at least 6 characters long.");
                
                // 3. Check if it begins with a capital letter
                if (!char.IsUpper(value[0]))
                    throw new InvalidItemDataException("Publisher must begin with a capital letter.");
                
                _publisher = value;
            }
        }

        // Public property for PublicationYear with validation
        public string PublicationYear
        {
            get { return _publicationYear; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Publication Year cannot be null or empty.");
                
                // 2. Check for length (must be exactly 4 characters)
                if (value.Length != 4)
                    throw new InvalidItemDataException("Publication Year must be exactly 4 digits (e.g., 1991, 2005).");
                
                // 3. Check if all characters are digits
                if (!int.TryParse(value, out int year))
                    throw new InvalidItemDataException("Publication Year must contain only numeric digits.");
                
                // 4. Additional check for reasonable year range
                if (year < 1450 || year > DateTime.Now.Year)
                    throw new InvalidItemDataException($"Publication Year must be between 1450 and {DateTime.Now.Year}.");
                
                _publicationYear = value;
            }
        }

        // Constructor
        protected Item(string title, string publisher, string publicationYear)
        {
            Title = title;
            Publisher = publisher;
            PublicationYear = publicationYear;
        }

        // Virtual method that can be overridden by derived classes
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Title: {Title}, Publisher: {Publisher}, Year: {PublicationYear}");
        }
    }
}