using System;

namespace Book_Fair_Management.UI
{
    public static class PurchaseMenu
    {
        public static void ShowPurchaseMenu()
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("           PURCHASE RECORDS               ");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 1. Record New Purchase");
                Console.WriteLine(" 2. View All Purchases");
                Console.WriteLine(" 3. Update Purchase Record");
                Console.WriteLine(" 4. Delete Purchase Record");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 0. Back to Main Menu");
                Console.WriteLine("==========================================");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        //AddPurchase();
                        break;
                    case "2":
                        //ViewPurchases();
                        break;
                    case "3":
                        //UpdatePurchase();
                        break;
                    case "4":
                        //DeletePurchase();
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