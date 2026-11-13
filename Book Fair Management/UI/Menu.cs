using System;
using static Book_Fair_Management.UI.VisitorMenu;
using static Book_Fair_Management.UI.AuthorMenu;
using static Book_Fair_Management.UI.BookMenu;
using static Book_Fair_Management.UI.PurchaseMenu;
using static Book_Fair_Management.UI.PublisherMenu;

namespace Book_Fair_Management.UI
{
    public static class Menu
    {
        public static void ShowMainMenu()
        {
            bool exitOption = false;

            while (!exitOption)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("       BOOK FAIR MANAGEMENT SYSTEM        ");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 1. Visitor Management");
                Console.WriteLine(" 2. Author Management");
                Console.WriteLine(" 3. Book Catalog");
                Console.WriteLine(" 4. Purchase Records");
                Console.WriteLine(" 5. Publisher Management");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 0. Exit Application");
                Console.WriteLine("==========================================");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowVisitorMenu();
                        break;
                    case "2":
                        ShowAuthorMenu();
                        break;
                    case "3":
                        ShowBookMenu();
                        break;
                    case "4":
                        ShowPurchaseMenu();
                        break;
                    case "5":
                        ShowPublisherMenu();
                        break;
                    case "0":
                        exitOption = true;
                        Console.WriteLine("\nExiting application...");
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