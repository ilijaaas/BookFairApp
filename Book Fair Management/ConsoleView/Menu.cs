using System;
using static Book_Fair_Management.UI.VisitorMenu;
using static Book_Fair_Management.UI.AuthorMenu;
using static Book_Fair_Management.UI.BookMenu;
using static Book_Fair_Management.UI.PurchaseMenu;
using static Book_Fair_Management.UI.PublisherMenu;
using static Book_Fair_Management.UI.AddressMenu;

namespace Book_Fair_Management.UI
{
    public static class Menu
    {
        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("       BOOK FAIR MANAGEMENT SYSTEM");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Visitors");
                Console.WriteLine("2. Authors");
                Console.WriteLine("3. Books");
                Console.WriteLine("4. Purchases");
                Console.WriteLine("5. Publishers");
                Console.WriteLine("6. Addresses");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("0. Exit");
                Console.Write("Select an entity: ");

                switch (Console.ReadLine())
                {
                    case "1": ShowVisitorMenu(); break;
                    case "2": ShowAuthorMenu(); break;
                    case "3": ShowBookMenu(); break;
                    case "4": ShowPurchaseMenu(); break;
                    case "5": ShowPublisherMenu(); break;
                    case "6": ShowAddressMenu(); break;
                    case "0": Console.WriteLine("Program finished."); return;
                    default:
                        Console.WriteLine("Unknown option. Press any key.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}