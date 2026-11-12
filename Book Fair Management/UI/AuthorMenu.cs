using Book_Fair_Management.Model;
using System;

namespace BookFairManagement.UI
{
    public static class AuthorMenu
    {
        public static void ShowAuthorMenu()
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("       AUTHOR MANAGEMENT        ");
                Console.WriteLine("================================");
                Console.WriteLine("1. Add New Author");
                Console.WriteLine("2. View All Authors");
                Console.WriteLine("3. Update Author");
                Console.WriteLine("4. Delete Author");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("--------------------------------");
                Console.Write("Select an option (0-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        //AddAuthor();
                        break;
                    case "2":
                        //ViewAuthors();
                        break;
                    case "3":
                       // UpdateAuthor();
                        break;
                    case "4":
                        // DeleteAuthor();
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