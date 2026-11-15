using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Book_Fair_Management.DAO;
using Book_Fair_Management.Model;

namespace Book_Fair_Management.UI
{

    internal static class CrudMenus
    {
        private static void Run(string title, Action add, Action update, Action delete, Action show)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("          " + title.ToUpperInvariant());
                Console.WriteLine("========================================");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Update");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Show all from file");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("0. Back");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": Execute(add); break;
                    case "2": Execute(update); break;
                    case "3": Execute(delete); break;
                    case "4": Execute(show); break;
                    case "0": return;
                    default: Pause("Unknown option."); break;
                }
            }
        }

        private static void Execute(Action action)
        {
            Console.Clear();
            try { action(); }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Operation failed: " + ex.Message);
            }
            Pause("Press any key to continue...");
        }

        private static void Pause(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadKey(true);
        }

        private static string ReadText(string label, string current = null)
        {
            while (true)
            {
                Console.Write(current == null ? label + ": " : label + " [" + current + "]: ");
                string value = Console.ReadLine();
                if (current != null && string.IsNullOrWhiteSpace(value)) return current;
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
                Console.WriteLine("This field is required.");
            }
        }

        private static int ReadInt(string label, int? current = null, int min = int.MinValue)
        {
            while (true)
            {
                Console.Write(current.HasValue ? label + " [" + current + "]: " : label + ": ");
                string raw = Console.ReadLine();
                if (current.HasValue && string.IsNullOrWhiteSpace(raw)) return current.Value;
                int value;
                if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) && value >= min)
                    return value;
                Console.WriteLine("Enter a whole number" + (min != int.MinValue ? " (minimum " + min + ")" : "") + ".");
            }
        }

        private static decimal ReadDecimal(string label, decimal? current = null, decimal min = 0)
        {
            while (true)
            {
                Console.Write(current.HasValue ? label + " [" + current.Value.ToString(CultureInfo.InvariantCulture) + "]: " : label + ": ");
                string raw = Console.ReadLine();
                if (current.HasValue && string.IsNullOrWhiteSpace(raw)) return current.Value;
                decimal value;
                if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value) && value >= min)
                    return value;
                Console.WriteLine("Enter a valid number (use a dot as the decimal separator).");
            }
        }

        private static DateTime ReadDate(string label, DateTime? current = null)
        {
            while (true)
            {
                Console.Write(current.HasValue ? label + " [yyyy-MM-dd, " + current.Value.ToString("yyyy-MM-dd") + "]: " : label + " (yyyy-MM-dd): ");
                string raw = Console.ReadLine();
                if (current.HasValue && string.IsNullOrWhiteSpace(raw)) return current.Value;
                DateTime value;
                if (DateTime.TryParseExact(raw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                    return value;
                Console.WriteLine("The date must be in the format yyyy-MM-dd.");
            }
        }

        private static int ReadRating(string label, int? current = null)
        {
            while (true)
            {
                Console.Write(current.HasValue ? label + " (1-5, 0 = no rating) [" + current + "]: " : label + " (1-5, 0 = no rating): ");
                string raw = Console.ReadLine();
                if (current.HasValue && string.IsNullOrWhiteSpace(raw)) return current.Value;
                int value;
                if (int.TryParse(raw, out value) && value >= 0 && value <= 5) return value;
                Console.WriteLine("The rating must be from 1 to 5, or 0 for no rating.");
            }
        }

        private static List<int> ReadIds(string label, List<int> current = null)
        {
            while (true)
            {
                Console.Write(current == null ? label + " (IDs separated by commas): " : label + " [" + string.Join(",", current) + "]: ");
                string raw = Console.ReadLine();
                if (current != null && string.IsNullOrWhiteSpace(raw)) return new List<int>(current);
                try
                {
                    List<int> ids = raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x.Trim(), CultureInfo.InvariantCulture)).Distinct().ToList();
                    if (ids.Count > 0) return ids;
                }
                catch (FormatException) { }
                Console.WriteLine("Enter at least one valid ID, e.g. 1,2,3.");
            }
        }

        private static void Result<T>(T item, string success, string failure) where T : class
        {
            Console.WriteLine(item == null ? failure : success);
            if (item != null) Console.WriteLine(item);
        }

        // ADDRESSES
        internal static void Addresses()
        {
            Run("Address management",
                AddAddress, UpdateAddress, DeleteAddress, ShowAddresses);
        }
        private static void AddAddress()
        {
            var dao = new AddressesDAO();
            var item = new Address(0, ReadText("Street"), ReadText("Number"), ReadText("City"), ReadText("Country"));
            Result(dao.AddAddress(item), "Address added.", "Address was not added.");
        }
        private static void UpdateAddress()
        {
            var dao = new AddressesDAO();
            int id = ReadInt("Address ID", null, 1);
            Address old = dao.GetAddressById(id);
            if (old == null) { Console.WriteLine("Address not found."); return; }
            var item = new Address(id, ReadText("Street", old.Street), ReadText("Number", old.Number),
                ReadText("City", old.City), ReadText("Country", old.Country));
            Result(dao.UpdateAddress(item), "Address updated.", "Update failed.");
        }
        private static void DeleteAddress()
        {
            var dao = new AddressesDAO();
            int id = ReadInt("Address ID to delete", null, 1);
            Result(dao.RemoveAddress(id), "Address deleted.", "Address does not exist or is used by an author/visitor.");
        }
        private static void ShowAddresses()
        {
            foreach (Address x in new AddressesDAO().GetAllAddresses())
                Console.WriteLine("ID: {0} | {1}", x.Id, x);
            if (!new AddressesDAO().GetAllAddresses().Any()) Console.WriteLine("The addresses file is empty.");
        }

        // AUTHORS
        internal static void Authors()
        {
            Run("Author management", AddAuthor, UpdateAuthor, DeleteAuthor, ShowAuthors);
        }
        private static void AddAuthor()
        {
            var dao = new AuthorsDAO();
            var item = new Author(0, ReadText("First name"), ReadText("Last name"), ReadDate("Date of birth"),
                ReadInt("Existing address ID", null, 1), ReadText("Phone"), ReadText("E-mail"),
                ReadText("ID card number"), ReadInt("Years of experience", null, 0));
            Result(dao.AddAuthor(item), "Author added.", "Author was not added. Check that the ID card number is unique and the address exists.");
        }
        private static void UpdateAuthor()
        {
            var dao = new AuthorsDAO();
            int id = ReadInt("Author ID", null, 1);
            Author old = dao.GetAuthorById(id);
            if (old == null) { Console.WriteLine("Author not found."); return; }
            var item = new Author(id, ReadText("First name", old.FirstName), ReadText("Last name", old.LastName),
                ReadDate("Date of birth", old.DateOfBirth), ReadInt("Address ID", old.AddressId, 1),
                ReadText("Phone", old.Phone), ReadText("E-mail", old.Email),
                ReadText("ID card number", old.IdCardNumber), ReadInt("Years of experience", old.YearsOfExperience, 0));
            Result(dao.UpdateAuthor(item), "Author updated.", "Update failed. Check the address, ID card number and the publisher head requirement.");
        }
        private static void DeleteAuthor()
        {
            Result(new AuthorsDAO().RemoveAuthor(ReadInt("Author ID to delete", null, 1)),
                "Author deleted.", "Author does not exist or is linked to a book/publisher.");
        }
        private static void ShowAuthors()
        {
            List<Author> items = new AuthorsDAO().GetAllAuthors();
            foreach (Author x in items)
                Console.WriteLine("ID: {0} | {1} {2} | Born: {3:yyyy-MM-dd} | Address: {4} | Phone: {5} | E-mail: {6} | ID card: {7} | Experience: {8}",
                    x.Id, x.FirstName, x.LastName, x.DateOfBirth, x.Address, x.Phone, x.Email, x.IdCardNumber, x.YearsOfExperience);
            if (!items.Any()) Console.WriteLine("The authors file is empty.");
        }

        // BOOKS
        internal static void Books()
        {
            Run("Book management", AddBook, UpdateBook, DeleteBook, ShowBooks);
        }
        private static void AddBook()
        {
            var dao = new BooksDAO();
            var item = new Book(ReadText("ISBN"), ReadText("Title"), ReadText("Genre"),
                ReadInt("Publication year", null, 0), ReadDecimal("Price", null, 0),
                ReadInt("Page count", null, 1), ReadText("Publisher code"), ReadIds("Authors"));
            Result(dao.AddBook(item), "Book added.", "Book was not added. Check that the ISBN is unique and the publisher and author IDs exist.");
        }
        private static void UpdateBook()
        {
            var dao = new BooksDAO();
            string isbn = ReadText("Book ISBN");
            Book old = dao.GetBookByIsbn(isbn);
            if (old == null) { Console.WriteLine("Book not found."); return; }
            var item = new Book(isbn, ReadText("Title", old.Title), ReadText("Genre", old.Genre),
                ReadInt("Publication year", old.PublicationYear, 0), ReadDecimal("Price", old.Price, 0),
                ReadInt("Page count", old.PageCount, 1), ReadText("Publisher code", old.PublisherCode),
                ReadIds("Authors", old.AuthorIds));
            Result(dao.UpdateBook(item), "Book updated.", "Update failed. Check the publisher and authors.");
        }
        private static void DeleteBook()
        {
            Result(new BooksDAO().RemoveBook(ReadText("ISBN of the book to delete")),
                "Book deleted.", "Book does not exist or is part of a purchase/wishlist.");
        }
        private static void ShowBooks()
        {
            List<Book> items = new BooksDAO().GetAllBooks();
            foreach (Book x in items)
                Console.WriteLine("ISBN: {0} | {1} | Genre: {2} | Year: {3} | Price: {4} | Pages: {5} | Publisher: {6} | Authors: {7}",
                    x.Isbn, x.Title, x.Genre, x.PublicationYear, x.Price, x.PageCount, x.PublisherCode, string.Join(",", x.AuthorIds));
            if (!items.Any()) Console.WriteLine("The books file is empty.");
        }

        // PUBLISHERS
        internal static void Publishers()
        {
            Run("Publisher management", AddPublisher, UpdatePublisher, DeletePublisher, ShowPublishers);
        }
        private static void AddPublisher()
        {
            var dao = new PublishersDAO();
            var item = new Publisher
            {
                Code = ReadText("Publisher code"),
                Name = ReadText("Publisher name"),
                HeadOfPublisherId = ReadInt("ID of the author who is the head (minimum 5 years of experience)", null, 1)
            };
            Result(dao.AddPublisher(item), "Publisher added.", "Publisher was not added. The head must be an existing author with at least 5 years of experience.");
        }
        private static void UpdatePublisher()
        {
            var dao = new PublishersDAO();
            string code = ReadText("Publisher code");
            Publisher old = dao.GetPublisherByCode(code);
            if (old == null) { Console.WriteLine("Publisher not found."); return; }
            var item = new Publisher
            {
                Code = code,
                Name = ReadText("Publisher name", old.Name),
                HeadOfPublisherId = ReadInt("ID of the head author", old.HeadOfPublisherId, 1)
            };
            Result(dao.UpdatePublisher(item), "Publisher updated.", "Update failed. Check that the head exists and has at least 5 years of experience.");
        }
        private static void DeletePublisher()
        {
            Result(new PublishersDAO().RemovePublisher(ReadText("Code of the publisher to delete")),
                "Publisher deleted.", "Publisher does not exist or has linked books.");
        }
        private static void ShowPublishers()
        {
            List<Publisher> items = new PublishersDAO().GetAllPublishers();
            foreach (Publisher x in items)
                Console.WriteLine("Code: {0} | {1} | Head author ID: {2} ({3})",
                    x.Code, x.Name, x.HeadOfPublisherId, x.HeadOfPublisher);
            if (!items.Any()) Console.WriteLine("The publishers file is empty.");
        }

        // VISITORS
        internal static void Visitors()
        {
            Run("Visitor management", AddVisitor, UpdateVisitor, DeleteVisitor, ShowVisitors);
        }
        private static Visitor ReadVisitor(int id, Visitor old = null)
        {
            string first = ReadText("First name", old == null ? null : old.FirstName);
            string last = ReadText("Last name", old == null ? null : old.LastName);
            DateTime dob = ReadDate("Date of birth", old == null ? (DateTime?)null : old.DateOfBirth);
            int addressId = ReadInt("Existing address ID", old == null ? (int?)null : old.AddressId, 1);
            string phone = ReadText("Phone", old == null ? null : old.ContactPhone);
            string email = ReadText("E-mail", old == null ? null : old.Email);
            string card = ReadText("Membership card number", old == null ? null : old.MembershipCardNumber);
            int year = ReadInt("Membership year", old == null ? (int?)null : old.CurrentMembershipYear, 0);
            VisitorStatus status;
            while (true)
            {
                string raw = ReadText("Status (Regular/VIP)", old == null ? "Regular" : old.Status.ToString());
                if (Enum.TryParse(raw, true, out status) && Enum.IsDefined(typeof(VisitorStatus), status)) break;
                Console.WriteLine("Allowed statuses are Regular and VIP.");
            }
            var visitor = new Visitor(id, first, last, dob, addressId, phone, email, card, year, status);
            if (old != null)
            {
                visitor.Wishlist = old.Wishlist;
                visitor.WishlistBookIsbns = old.WishlistBookIsbns;
            }
            return visitor;
        }
        private static void AddVisitor()
        {
            Result(new VisitorsDAO().AddVisitor(ReadVisitor(0)), "Visitor added.", "Visitor was not added. Check the card number, address and linked books.");
        }
        private static void UpdateVisitor()
        {
            var dao = new VisitorsDAO();
            int id = ReadInt("Visitor ID", null, 1);
            Visitor old = dao.GetVisitorById(id);
            if (old == null) { Console.WriteLine("Visitor not found."); return; }
            Result(dao.UpdateVisitor(ReadVisitor(id, old)), "Visitor updated.", "Update failed. Check the address and that the card number is unique.");
        }
        private static void DeleteVisitor()
        {
            Result(new VisitorsDAO().RemoveVisitor(ReadInt("Visitor ID to delete", null, 1)),
                "Visitor deleted.", "Visitor does not exist or has recorded purchases.");
        }
        private static void ShowVisitors()
        {
            List<Visitor> items = new VisitorsDAO().GetAllVisitors();
            foreach (Visitor x in items)
                Console.WriteLine("ID: {0} | Card: {1} | {2} {3} | Born: {4:yyyy-MM-dd} | Address: {5} | Status: {6} | Phone: {7} | E-mail: {8}",
                    x.Id, x.MembershipCardNumber, x.FirstName, x.LastName, x.DateOfBirth, x.Address, x.Status, x.ContactPhone, x.Email);
            if (!items.Any()) Console.WriteLine("The visitors file is empty.");
        }

        // PURCHASES
        internal static void Purchases()
        {
            Run("Purchase management", AddPurchase, UpdatePurchase, DeletePurchase, ShowPurchases);
        }
        private static Purchase ReadPurchase(int id, Purchase old = null)
        {
            int visitorId = ReadInt("Existing visitor ID", old == null ? (int?)null : old.VisitorId, 1);
            string isbn = ReadText("Existing book ISBN", old == null ? null : old.BookIsbn);
            DateTime date = ReadDate("Purchase date", old == null ? (DateTime?)null : old.PurchaseDate);
            int rating = ReadRating("Rating", old == null ? (int?)0 : (old.Rating ?? 0));
            string comment = ReadText("Comment (enter - if there is no comment)", old == null ? "-" : (string.IsNullOrEmpty(old.Comment) ? "-" : old.Comment));
            if (comment == "-") comment = string.Empty;
            return new Purchase(id, visitorId, isbn, date, rating == 0 ? (int?)null : rating, comment);
        }
        private static void AddPurchase()
        {
            Result(new PurchasesDAO().AddPurchase(ReadPurchase(0)), "Purchase recorded.", "Purchase was not recorded. Check that the visitor and book exist.");
        }
        private static void UpdatePurchase()
        {
            var dao = new PurchasesDAO();
            int id = ReadInt("Purchase ID", null, 1);
            Purchase old = dao.GetPurchaseById(id);
            if (old == null) { Console.WriteLine("Purchase not found."); return; }
            Result(dao.UpdatePurchase(ReadPurchase(id, old)), "Purchase updated.", "Purchase update failed.");
        }
        private static void DeletePurchase()
        {
            Result(new PurchasesDAO().RemovePurchase(ReadInt("Purchase ID to delete", null, 1)),
                "Purchase deleted.", "Purchase not found.");
        }
        private static void ShowPurchases()
        {
            List<Purchase> items = new PurchasesDAO().GetAllPurchases();
            foreach (Purchase x in items)
                Console.WriteLine("ID: {0} | Visitor: {1} | Book: {2} | Date: {3:yyyy-MM-dd} | Rating: {4} | Comment: {5}",
                    x.Id, x.VisitorId, x.BookIsbn, x.PurchaseDate, x.Rating.HasValue ? x.Rating.Value.ToString() : "no rating", x.Comment);
            if (!items.Any()) Console.WriteLine("The purchases file is empty.");
        }
    }

    public static class AddressMenu { public static void ShowAddressMenu() { CrudMenus.Addresses(); } }
    public static class AuthorMenu { public static void ShowAuthorMenu() { CrudMenus.Authors(); } }
    public static class BookMenu { public static void ShowBookMenu() { CrudMenus.Books(); } }
    public static class PublisherMenu { public static void ShowPublisherMenu() { CrudMenus.Publishers(); } }
    public static class VisitorMenu { public static void ShowVisitorMenu() { CrudMenus.Visitors(); } }
    public static class PurchaseMenu { public static void ShowPurchaseMenu() { CrudMenus.Purchases(); } }
}
