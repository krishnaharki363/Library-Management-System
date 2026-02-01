using System;
using System.Text.Json.Serialization;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem.Model
{
    /// <summary>
    /// Magazine class derived from LibraryItemBase
    /// </summary>
    public class Magazine : LibraryItemBase
    {
        // Private field with proper naming convention
        private string _issueNumber = null!;

        // Public property for IssueNumber with validation
        public string IssueNumber
        {
            get { return _issueNumber; }
            set
            {
                // 1. Check for Null
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Issue Number cannot be null or empty.");
                
                // 2. Check if it's a valid number
                if (!int.TryParse(value, out int issueNum))
                    throw new InvalidItemDataException("Issue Number must be a valid number.");
                
                // 3. Check if it's a positive number
                if (issueNum <= 0)
                    throw new InvalidItemDataException("Issue Number must be greater than zero.");
                
                _issueNumber = value;
            }
        }

        // Constructor
        public Magazine(string title, string publisher, string publicationYear, string issueNumber)
            : base(title, publisher, publicationYear)
        {
            IssueNumber = issueNumber;
        }

        // Type property for JSON serialization
        [JsonIgnore]
        public string Type => "Magazine";

        // Override DisplayInfo to show Magazine-specific information
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Issue Number: {IssueNumber}");
            Console.WriteLine($"Type: Magazine");
            Console.WriteLine("-----------------------------------");
        }
    }
}
