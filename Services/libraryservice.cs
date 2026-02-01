using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem.Service
{
    /// <summary>
    /// Service layer to manage library items
    /// </summary>
    public class LibraryService
    {
        // Private list to store items
        private List<ILibraryItem> _items = new List<ILibraryItem>();
        private const string DataFilePath = "libraryData.json";

        // Constructor - load data on initialization
        public LibraryService()
        {
            LoadData();
        }

        // Method to add item to the library
        public void AddItem(ILibraryItem item)
        {
            // Check for duplicate entries
            foreach (var existingItem in _items)
            {
                if (existingItem.Title == item.Title &&
                    existingItem.Publisher == item.Publisher &&
                    existingItem.PublicationYear == item.PublicationYear)
                {
                    throw new DuplicateEntryException("Item already exists in the library.");
                }
            }

            // If no duplicate found, add the item
            _items.Add(item);
            SaveData(); // Save data after adding
            Console.WriteLine($"\n✓ Item added successfully:");
            Console.WriteLine($"  Title: {item.Title}");
            Console.WriteLine($"  Publisher: {item.Publisher}");
            Console.WriteLine($"  Publication Year: {item.PublicationYear}");
        }

        // Method to remove item from the library
        public void RemoveItem(string title)
        {
            ILibraryItem? itemToRemove = null;
            foreach (var item in _items)
            {
                if (item.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    itemToRemove = item;
                    break;
                }
            }

            if (itemToRemove == null)
            {
                throw new InvalidItemDataException($"Item with title '{title}' not found in the library.");
            }

            _items.Remove(itemToRemove);
            SaveData(); // Save data after removing
            Console.WriteLine($"\n✓ Item removed successfully:");
            Console.WriteLine($"  Title: {itemToRemove.Title}");
            Console.WriteLine($"  Publisher: {itemToRemove.Publisher}");
            Console.WriteLine($"  Publication Year: {itemToRemove.PublicationYear}");
        }

        // Method to get item count
        public int GetItemCount()
        {
            return _items.Count;
        }

        // Method to display all items
        public void DisplayAllItems()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("No items in the library.");
                return;
            }
            Console.WriteLine("\nAll Items in Library:");
            foreach (var item in _items)
            {
                item.DisplayInfo();
            }
        }

        // File I/O Methods
        private void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new LibraryItemJsonConverter() }
                };

                string jsonData = JsonSerializer.Serialize(_items, options);
                File.WriteAllText(DataFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    string jsonData = File.ReadAllText(DataFilePath);
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new LibraryItemJsonConverter() }
                    };

                    var loadedItems = JsonSerializer.Deserialize<List<ILibraryItem>>(jsonData, options);
                    if (loadedItems != null)
                    {
                        _items = loadedItems;
                        Console.WriteLine($"Loaded {_items.Count} items from file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        // Search Methods
        public List<ILibraryItem> SearchByTitle(string title)
        {
            return _items.FindAll(item =>
                item.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        public List<ILibraryItem> SearchByAuthor(string author)
        {
            return _items.OfType<Book>()
                         .Where(book => book.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                         .Cast<ILibraryItem>()
                         .ToList();
        }

        // Sorting Methods
        public void SortByTitle()
        {
            _items.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase));
        }

        public void SortByAuthor()
        {
            var books = _items.OfType<Book>().ToList();
            var nonBooks = _items.Where(item => !(item is Book)).ToList();

            books.Sort((a, b) => string.Compare(a.Author, b.Author, StringComparison.OrdinalIgnoreCase));

            _items = books.Cast<ILibraryItem>().Concat(nonBooks).ToList();
        }

        public void SortByPublicationYear()
        {
            _items.Sort((a, b) => string.Compare(a.PublicationYear, b.PublicationYear));
        }

        // Display sorted items
        public void DisplaySortedItems(string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case "title":
                    SortByTitle();
                    break;
                case "author":
                    SortByAuthor();
                    break;
                case "year":
                    SortByPublicationYear();
                    break;
                default:
                    Console.WriteLine("Invalid sort criteria. Use 'title', 'author', or 'year'.");
                    return;
            }

            Console.WriteLine($"\nItems sorted by {sortBy}:");
            DisplayAllItems();
        }

        // Display search results
        public void DisplaySearchResults(List<ILibraryItem> results, string searchTerm, string searchType)
        {
            if (results.Count == 0)
            {
                Console.WriteLine($"No items found for {searchType}: '{searchTerm}'");
                return;
            }

            Console.WriteLine($"\nSearch results for {searchType} '{searchTerm}' ({results.Count} items found):");
            foreach (var item in results)
            {
                item.DisplayInfo();
            }
        }
    }

    // JSON Converter for polymorphic serialization
    public class LibraryItemJsonConverter : JsonConverter<ILibraryItem>
    {
        public override ILibraryItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var type = root.GetProperty("Type").GetString();

                var json = root.GetRawText();
                return type switch
                {
                    "Book" => JsonSerializer.Deserialize<Book>(json, options)!,
                    "Magazine" => JsonSerializer.Deserialize<Magazine>(json, options)!,
                    "Newspaper" => JsonSerializer.Deserialize<Newspaper>(json, options)!,
                    _ => throw new JsonException($"Unknown item type: {type}")
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, ILibraryItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write the type discriminator
            if (value is Book)
                writer.WriteString("Type", "Book");
            else if (value is Magazine)
                writer.WriteString("Type", "Magazine");
            else if (value is Newspaper)
                writer.WriteString("Type", "Newspaper");

            // Write common properties
            writer.WriteString("Title", value.Title);
            writer.WriteString("Publisher", value.Publisher);
            writer.WriteString("PublicationYear", value.PublicationYear);

            // Write type-specific properties
            if (value is Book book)
            {
                writer.WriteString("Author", book.Author);
            }
            else if (value is Magazine magazine)
            {
                writer.WriteString("IssueNumber", magazine.IssueNumber);
            }
            else if (value is Newspaper newspaper)
            {
                writer.WriteString("Edition", newspaper.Edition);
            }

            writer.WriteEndObject();
        }
    }
}
