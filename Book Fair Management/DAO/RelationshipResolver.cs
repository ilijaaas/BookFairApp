using System.Linq;
using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;

namespace Book_Fair_Management.DAO
{
    internal static class RelationshipResolver
    {
        internal static void Resolve(Visitor visitor)
        {
            visitor.Address = new Storage<Address>("addresses.txt").Load()
                .FirstOrDefault(a => a.Id == visitor.AddressId);

            var books = new Storage<Book>("books.txt").Load();
            visitor.Purchases = new Storage<Purchase>("purchases.txt").Load()
                .Where(p => p.VisitorId == visitor.Id)
                .Select(p =>
                {
                    p.Visitor = visitor;
                    p.Book = books.FirstOrDefault(b => b.Isbn == p.BookIsbn);
                    if (p.Book != null) Resolve(p.Book);
                    return p;
                }).ToList();

            visitor.Wishlist = books
                .Where(b => visitor.WishlistBookIsbns.Contains(b.Isbn))
                .ToList();
        }

        internal static void Resolve(Author author)
        {
            author.Address = new Storage<Address>("addresses.txt").Load()
                .FirstOrDefault(a => a.Id == author.AddressId);

            author.Books = new Storage<Book>("books.txt").Load()
                .Where(b => b.AuthorIds.Contains(author.Id)).ToList();
        }

        internal static void Resolve(Book book)
        {
            var authors = new Storage<Author>("authors.txt").Load();
            book.Authors = authors.Where(a => book.AuthorIds.Contains(a.Id)).ToList();
            book.Publisher = new Storage<Publisher>("publishers.txt").Load()
                .FirstOrDefault(p => p.Code == book.PublisherCode);
            if (book.Publisher != null)
            {
                book.Publisher.HeadOfPublisher = authors
                    .FirstOrDefault(a => a.Id == book.Publisher.HeadOfPublisherId);
            }

            var visitors = new Storage<Visitor>("visitors.txt").Load();
            var purchases = new Storage<Purchase>("purchases.txt").Load();
            book.PurchasedBy = purchases
                .Where(p => p.BookIsbn == book.Isbn)
                .Select(p => visitors.FirstOrDefault(v => v.Id == p.VisitorId))
                .Where(v => v != null).ToList();

            book.WishlistedBy = visitors
                .Where(v => v.WishlistBookIsbns.Contains(book.Isbn))
                .ToList();
        }

        internal static void Resolve(Publisher publisher)
        {
            publisher.HeadOfPublisher = new Storage<Author>("authors.txt").Load()
                .FirstOrDefault(a => a.Id == publisher.HeadOfPublisherId);

            var books = new Storage<Book>("books.txt").Load()
                .Where(b => b.PublisherCode == publisher.Code).ToList();
            publisher.Books = books;

            var authorIds = books.SelectMany(b => b.AuthorIds).Distinct().ToList();
            publisher.Authors = new Storage<Author>("authors.txt").Load()
                .Where(a => authorIds.Contains(a.Id)).ToList();
        }

        internal static void Resolve(Purchase purchase)
        {
            purchase.Visitor = new Storage<Visitor>("visitors.txt").Load()
                .FirstOrDefault(v => v.Id == purchase.VisitorId);
            purchase.Book = new Storage<Book>("books.txt").Load()
                .FirstOrDefault(b => b.Isbn == purchase.BookIsbn);
            if (purchase.Book != null) Resolve(purchase.Book);
        }
    }
}
