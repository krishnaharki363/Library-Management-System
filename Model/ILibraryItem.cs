using System;

namespace LibraryManagementSystem.Model
{
    /// <summary>
    /// Interface defining common properties and methods for library items
    /// </summary>
    public interface ILibraryItem
    {
        string Title { get; set; }
        string Publisher { get; set; }
        string PublicationYear { get; set; }
        void DisplayInfo();
    }
}