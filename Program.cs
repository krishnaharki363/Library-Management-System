using System;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Service;
using LibraryManagementSystem.CustomException;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║         LIBRARY MANAGEMENT SYSTEM              ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            LibraryService libraryService = new LibraryService();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Add a Magazine");
                Console.WriteLine("3. Add a Newspaper");
                Console.WriteLine("4. Remove an Item");
                Console.WriteLine("5. Display All Items");
                Console.WriteLine("6. Search by Title");
                Console.WriteLine("7. Search by Author");
                Console.WriteLine("8. Sort Items");
                Console.WriteLine("9. Exit");
                Console.Write("Choose an option (1-9): ");

                string choice = Console.ReadLine()!;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddBook(libraryService);
                            break;
                        case "2":
                            AddMagazine(libraryService);
                            break;
                        case "3":
                            AddNewspaper(libraryService);
                            break;
                        case "4":
                            RemoveItem(libraryService);
                            break;
                        case "5":
                            libraryService.DisplayAllItems();
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case "6":
                            SearchByTitle(libraryService);
                            break;
                        case "7":
                            SearchByAuthor(libraryService);
                            break;
                        case "8":
                            SortItems(libraryService);
                            break;
                        case "9":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please select 1-9.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n✗ ERROR: {ex.Message}");
                }
            }

            Console.WriteLine("\nThank you for using the Library Management System!");
            Console.WriteLine($"Final item count: {libraryService.GetItemCount()}");

            try
            {
                // Test cases for validation
                // Test 1: Title too short
                Console.WriteLine("\n[TEST 1] Creating book with title too short:");
                try
                {
                    Book invalidBook1 = new Book(
                        "IT",  // Only 2 characters
                        "Viking Press",
                        "1986",
                        "Stephen King"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 2: Title not starting with capital letter
                Console.WriteLine("\n[TEST 2] Creating book with title not starting with capital:");
                try
                {
                    Book invalidBook2 = new Book(
                        "the great gatsby",  // Lowercase start
                        "Scribner",
                        "1925",
                        "F. Scott Fitzgerald"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 3: Publisher too short
                Console.WriteLine("\n[TEST 3] Creating book with publisher too short (less than 6 chars):");
                try
                {
                    Book invalidBook3 = new Book(
                        "Valid Title Here",
                        "ABC",  // Only 3 characters
                        "2020",
                        "Valid Author"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 4: Publisher not starting with capital letter
                Console.WriteLine("\n[TEST 4] Creating book with publisher not starting with capital:");
                try
                {
                    Book invalidBook4 = new Book(
                        "Valid Title",
                        "penguin books",  // Lowercase start
                        "2020",
                        "Valid Author"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 5: Invalid publication year (not 4 digits)
                Console.WriteLine("\n[TEST 5] Creating book with invalid year (not 4 digits):");
                try
                {
                    Book invalidBook5 = new Book(
                        "Valid Title",
                        "Valid Publisher",
                        "202",  // Only 3 digits
                        "Valid Author"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 6: Invalid publication year (future year)
                Console.WriteLine("\n[TEST 6] Creating book with future year:");
                try
                {
                    Book invalidBook6 = new Book(
                        "Valid Title",
                        "Valid Publisher",
                        "3000",  // Future year
                        "Valid Author"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 7: Author too short
                Console.WriteLine("\n[TEST 7] Creating book with author name too short:");
                try
                {
                    Book invalidBook7 = new Book(
                        "Valid Title",
                        "Valid Publisher",
                        "2020",
                        "Tom"  // Only 3 characters
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 8: Author not starting with capital
                Console.WriteLine("\n[TEST 8] Creating book with author not starting with capital:");
                try
                {
                    Book invalidBook8 = new Book(
                        "Valid Title",
                        "Valid Publisher",
                        "2020",
                        "john smith"  // Lowercase start
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 9: Invalid issue number (negative)
                Console.WriteLine("\n[TEST 9] Creating magazine with invalid issue number:");
                try
                {
                    Magazine invalidMag1 = new Magazine(
                        "Valid Magazine Title",
                        "Valid Publisher",
                        "2024",
                        "-5"  // Negative number
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 10: Duplicate entry
                Console.WriteLine("\n[TEST 10] Adding duplicate item:");
                try
                {
                    Book duplicateBook = new Book(
                        "The Great Gatsby",
                        "Scribner",
                        "1925",
                        "F. Scott Fitzgerald"
                    );
                    libraryService.AddItem(duplicateBook);
                }
                catch (DuplicateEntryException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Test 11: Null title
                Console.WriteLine("\n[TEST 11] Creating book with null/empty title:");
                try
                {
                    Book invalidBook11 = new Book(
                        "",  // Empty title
                        "Valid Publisher",
                        "2020",
                        "Valid Author"
                    );
                }
                catch (InvalidItemDataException ex)
                {
                    Console.WriteLine($"✗ ERROR CAUGHT: {ex.Message}");
                }

                // Display final library state
                libraryService.DisplayAllItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n✗ CRITICAL ERROR: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                // Cleanup and final statistics
                Console.WriteLine("\n\n╔════════════════════════════════════════════════╗");
                Console.WriteLine("║              PROGRAM COMPLETED                 ║");
                Console.WriteLine("╚════════════════════════════════════════════════╝");
                
                Console.WriteLine($"\nFinal Statistics:");
                Console.WriteLine($"  Total items in library: {libraryService.GetItemCount()}");
                
                Console.WriteLine("\n\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        static void AddBook(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Add a Book ---");
            Console.Write("Title: ");
            string title = Console.ReadLine()!;
            Console.Write("Publisher: ");
            string publisher = Console.ReadLine()!;
            Console.Write("Publication Year: ");
            string year = Console.ReadLine()!;
            Console.Write("Author: ");
            string author = Console.ReadLine()!;

            Book book = new Book(title, publisher, year, author);
            libraryService.AddItem(book);
        }

        static void AddMagazine(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Add a Magazine ---");
            Console.Write("Title: ");
            string title = Console.ReadLine()!;
            Console.Write("Publisher: ");
            string publisher = Console.ReadLine()!;
            Console.Write("Publication Year: ");
            string year = Console.ReadLine()!;
            Console.Write("Issue Number: ");
            string issueNumber = Console.ReadLine()!;

            Magazine magazine = new Magazine(title, publisher, year, issueNumber);
            libraryService.AddItem(magazine);
        }

        static void RemoveItem(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Remove an Item ---");
            Console.Write("Enter the title of the item to remove: ");
            string title = Console.ReadLine()!;

            libraryService.RemoveItem(title);
        }

        static void AddNewspaper(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Add a Newspaper ---");
            Console.Write("Title: ");
            string title = Console.ReadLine()!;
            Console.Write("Publisher: ");
            string publisher = Console.ReadLine()!;
            Console.Write("Publication Year: ");
            string year = Console.ReadLine()!;
            Console.Write("Edition: ");
            string edition = Console.ReadLine()!;

            Newspaper newspaper = new Newspaper(title, publisher, year, edition);
            libraryService.AddItem(newspaper);
        }

        static void SearchByTitle(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Search by Title ---");
            Console.Write("Enter title to search: ");
            string title = Console.ReadLine()!;

            var results = libraryService.SearchByTitle(title);
            libraryService.DisplaySearchResults(results, title, "title");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void SearchByAuthor(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Search by Author ---");
            Console.Write("Enter author name to search: ");
            string author = Console.ReadLine()!;

            var results = libraryService.SearchByAuthor(author);
            libraryService.DisplaySearchResults(results, author, "author");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void SortItems(LibraryService libraryService)
        {
            Console.WriteLine("\n--- Sort Items ---");
            Console.WriteLine("Sort by:");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Author");
            Console.WriteLine("3. Publication Year");
            Console.Write("Choose sort criteria (1-3): ");
            string sortChoice = Console.ReadLine()!;

            string sortBy = sortChoice switch
            {
                "1" => "title",
                "2" => "author",
                "3" => "year",
                _ => ""
            };

            if (!string.IsNullOrEmpty(sortBy))
            {
                libraryService.DisplaySortedItems(sortBy);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}