using System;

namespace Book_Fair_Management.UI
{
    public static class VisitorMenu
    {
        public static void ShowVisitorMenu()
        {
            bool backToMain = false;
            
            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("           VISITOR MANAGEMENT             ");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 1. Add New Visitor");
                Console.WriteLine(" 2. View All Visitors");
                Console.WriteLine(" 3. Update Visitor");
                Console.WriteLine(" 4. Delete Visitor");
                Console.WriteLine("==========================================");
                Console.WriteLine(" 0. Back to Main Menu");
                Console.WriteLine("==========================================");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        //AddVisitor();
                        break;
                    case "2":
                        //ViewVisitors();
                        break;
                    case "3":
                        //UpdateVisitor();
                        break;
                    case "4":
                        //DeleteVisitor();
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