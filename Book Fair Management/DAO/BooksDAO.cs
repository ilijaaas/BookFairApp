using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class BooksDAO
    {
        private readonly List<Book> _books;
        private readonly Storage<Book> _storage;

        public BooksDAO()
        {
            Console.WriteLine("BooksDAO konstruktor pokrenut!");

            _storage = new Storage<Book>("books.txt");
            _books = _storage.Load();
        }

        public Book? AddBook(Book book)
        {
            if (book == null || _books.Any(b => b.Isbn == book.Isbn))
                return null;

            if (!new Storage<Publisher>("publishers.txt").Load().Any(p => p.Code == book.PublisherCode))
                return null;

            var authorIds = book.AuthorIds ?? new List<int>();
            var knownAuthorIds = new Storage<Author>("authors.txt").Load().Select(a => a.Id).ToHashSet();
            if (authorIds.Count == 0 || authorIds.Any(id => !knownAuthorIds.Contains(id)))
                return null;

            book.AuthorIds = authorIds.Distinct().ToList();
            _books.Add(book);
            _storage.Save(_books);
            return book;
        }

        public Book? UpdateBook(Book book)
        {
            if (book == null) return null;
            Book? oldBook = _books.Find(b => b.Isbn == book.Isbn);
            if (oldBook is null) return null;

            if (!new Storage<Publisher>("publishers.txt").Load().Any(p => p.Code == book.PublisherCode))
                return null;
            var knownAuthorIds = new Storage<Author>("authors.txt").Load().Select(a => a.Id).ToHashSet();
            if (book.AuthorIds == null || book.AuthorIds.Count == 0 ||
                book.AuthorIds.Any(id => !knownAuthorIds.Contains(id)))
                return null;

            oldBook.Title = book.Title;
            oldBook.Genre = book.Genre;
            oldBook.PublicationYear = book.PublicationYear;
            oldBook.Price = book.Price;
            oldBook.PageCount = book.PageCount;
            oldBook.PublisherCode = book.PublisherCode;
            oldBook.AuthorIds = book.AuthorIds.Distinct().ToList();

            _storage.Save(_books);
            return oldBook;
        }

        public Book? RemoveBook(string isbn)
        {
            Book? book = _books.Find(b => b.Isbn == isbn);
            if (book == null) return null;
            bool usedInPurchase = new Storage<Purchase>("purchases.txt").Load()
                .Any(p => p.BookIsbn == isbn);
            bool inWishlist = new Storage<Visitor>("visitors.txt").Load()
                .Any(v => v.WishlistBookIsbns.Contains(isbn));
            if (usedInPurchase || inWishlist)
                return null;

            _books.Remove(book);
            _storage.Save(_books);
            return book;
        }

        public Book? GetBookByIsbn(string isbn)
        {
            Book book = _books.Find(b => b.Isbn == isbn);
            if (book != null) RelationshipResolver.Resolve(book);
            return book;
        }

        public List<Book> GetAllBooks()
        {
            foreach (Book book in _books)
                RelationshipResolver.Resolve(book);
            return _books;
        }

        public List<Book> GetAllBooks(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Book>();

IEnumerable<Book> books = _books;

            switch (sortCriteria)
            {
                case "Isbn":
                    books = sortDirection == SortDirection.Ascending ? _books.OrderBy(x => x.Isbn) : _books.OrderByDescending(x => x.Isbn);
                    break;
                case "Title":
                    books = sortDirection == SortDirection.Ascending ? _books.OrderBy(x => x.Title) : _books.OrderByDescending(x => x.Title);
                    break;
                case "Price":
                    books = sortDirection == SortDirection.Ascending ? _books.OrderBy(x => x.Price) : _books.OrderByDescending(x => x.Price);
                    break;
                case "PublicationYear":
                    books = sortDirection == SortDirection.Ascending ? _books.OrderBy(x => x.PublicationYear) : _books.OrderByDescending(x => x.PublicationYear);
                    break;
                default:
                    books = _books.OrderBy(x => x.Isbn);
                    break;
            }

            
            books = books.Skip((page - 1) * pageSize).Take(pageSize);

            List<Book> result = books.ToList();
            foreach (Book item in result)
                RelationshipResolver.Resolve(item);
            return result;
        }
    }
}