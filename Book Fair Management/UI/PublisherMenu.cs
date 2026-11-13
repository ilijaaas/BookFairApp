using System;

namespace Book_Fair_Management.UI
{
    public static class PublisherMenu
    {
        public static void ShowPublisherMenu()
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("          PUBLISHER MANAGEMENT            ");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 1. Add New Publisher");
                Console.WriteLine(" 2. View All Publishers");
                Console.WriteLine(" 3. Update Publisher");
                Console.WriteLine(" 4. Delete Publisher");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 0. Back to Main Menu");
                Console.WriteLine("==========================================");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        //AddPublisher();
                        break;
                    case "2":
                        //ViewPublishers();
                        break;
                    case "3":
                        //UpdatePublisher();
                        break;
                    case "4":
                        //DeletePublisher();
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