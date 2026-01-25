using System;
using System.Collections.Generic;
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
        private List<Item> _items = new List<Item>();

        // Method to add item to the library
        public void AddItem(Item item)
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
            Console.WriteLine($"\n✓ Item added successfully:");
            Console.WriteLine($"  Title: {item.Title}");
            Console.WriteLine($"  Publisher: {item.Publisher}");
            Console.WriteLine($"  Publication Year: {item.PublicationYear}");
        }

        // Method to remove item from the library
        public void RemoveItem(string title)
        {
            Item? itemToRemove = null;
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
    }
}
