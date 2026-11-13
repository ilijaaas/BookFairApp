using System;
using System.Collections.Generic;
using System.Text;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PublicationYear { get; set; }
        public decimal Price { get; set; }
        public int PageCount { get; set; }
        public string PublisherCode { get; set; }
        public int AuthorIds { get; set;  }
        public List<Author> Authors { get; set; } = new List<Author>();
        public Publisher Publisher { get; set; }
        public List<Visitor> PurchasedBy { get; set; } = new List<Visitor>();
        public List<Visitor> WishlistedBy { get; set; } = new List<Visitor>();

        public Book() { }

        public Book(string isbn, string title, string genre, int publicationYear,
                    decimal price, int pageCount, Publisher publisher)
        {
            Isbn = isbn;
            Title = title;
            Genre = genre;
            PublicationYear = publicationYear;
            Price = price;
            PageCount = pageCount;
            Publisher = publisher;
        }
    }
}
