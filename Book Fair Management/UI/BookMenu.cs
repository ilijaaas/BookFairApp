using System;

namespace BookFairManagement.UI
{
    public static class BookMenu
    {
        public static void ShowBookMenu()
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("        BOOK MANAGEMENT         ");
                Console.WriteLine("================================");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Update Book");
                Console.WriteLine("4. Delete Book");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("================================");
                Console.Write("Select an option (0-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                       //  AddBook();
                        break;
                    case "2":
                        // ViewBooks();
                        break;
                    case "3":
                        // UpdateBook();
                        break;
                    case "4":
                        // DeleteBook();
                        break;
                    case "0":
                        backToMain = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice! Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

    }
}
