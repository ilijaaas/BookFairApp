using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Book : ISerializable
    {
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public decimal Price { get; set; }
        public int PageCount { get; set; }
        public string PublisherCode { get; set; } = string.Empty;
        public List<int> AuthorIds { get; set; } = new List<int>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public Publisher Publisher { get; set; }
        public List<Visitor> PurchasedBy { get; set; } = new List<Visitor>();
        public List<Visitor> WishlistedBy { get; set; } = new List<Visitor>();

        public Book() { }

        public Book(string isbn, string title, string genre, int publicationYear,
                    decimal price, int pageCount, string publisherCode, IEnumerable<int> authorIds)
        {
            Isbn = isbn;
            Title = title;
            Genre = genre;
            PublicationYear = publicationYear;
            Price = price;
            PageCount = pageCount;
            PublisherCode = publisherCode;
            AuthorIds = authorIds?.Distinct().ToList() ?? new List<int>();
        }

       
        public Book(string isbn, string title, string genre, int publicationYear,
                    decimal price, int pageCount, string publisherCode, int authorId)
            : this(isbn, title, genre, publicationYear, price, pageCount, publisherCode, new[] { authorId })
        {
        }

        public override string ToString() => $"{Title} (ISBN: {Isbn}) - {Price:C}";

        public string[] ToCSV() => new[]
        {
            Isbn ?? string.Empty,
            Title ?? string.Empty,
            Genre ?? string.Empty,
            PublicationYear.ToString(CultureInfo.InvariantCulture),
            Price.ToString(CultureInfo.InvariantCulture),
            PageCount.ToString(CultureInfo.InvariantCulture),
            PublisherCode ?? string.Empty,
            string.Join(",", (AuthorIds ?? new List<int>()).Distinct())
        };

        public void FromCSV(string[] values)
        {
            if (values == null || values.Length != 8)
                throw new FormatException($"Invalid book record: expected 8 fields, but received {values?.Length ?? 0}.");

            Isbn = values[0];
            Title = values[1];
            Genre = values[2];
            PublicationYear = int.Parse(values[3], CultureInfo.InvariantCulture);
            Price = decimal.Parse(values[4], CultureInfo.InvariantCulture);
            PageCount = int.Parse(values[5], CultureInfo.InvariantCulture);
            PublisherCode = values[6];

          
            AuthorIds = string.IsNullOrWhiteSpace(values[7])
                ? new List<int>()
                : values[7].Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x, CultureInfo.InvariantCulture))
                    .Distinct()
                    .ToList();
        }
    }
}
