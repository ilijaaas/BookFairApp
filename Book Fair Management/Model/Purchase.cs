using System;
using System.Globalization;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Purchase : ISerializable
    {
        private int? _rating;

        public int Id { get; set; }
        public int VisitorId { get; set; }
        public string BookIsbn { get; set; } = string.Empty;
        public Visitor Visitor { get; set; }
        public Book Book { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Comment { get; set; } = string.Empty;

        public int? Rating
        {
            get => _rating;
            set
            {
                if (value.HasValue && (value < 1 || value > 5))
                    throw new ArgumentOutOfRangeException(nameof(value), "The rating must be between 1 and 5.");
                _rating = value;
            }
        }

        public Purchase() { }

        public Purchase(int id, int visitorId, string bookIsbn, DateTime purchaseDate,
                        int? rating = null, string comment = null)
        {
            Id = id;
            VisitorId = visitorId;
            BookIsbn = bookIsbn;
            PurchaseDate = purchaseDate;
            Rating = rating;
            Comment = comment ?? string.Empty;
        }

        public override string ToString() =>
            $"Purchase ID: {Id} | Date: {PurchaseDate:yyyy-MM-dd} | Rating: {(Rating.HasValue ? Rating.ToString() : "N/A")}";

        public string[] ToCSV() => new[]
        {
            Id.ToString(CultureInfo.InvariantCulture),
            VisitorId.ToString(CultureInfo.InvariantCulture),
            BookIsbn ?? string.Empty,
            PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            Rating.HasValue ? Rating.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
            Comment ?? string.Empty
        };

        public void FromCSV(string[] values)
        {
            if (values == null || values.Length != 6)
                throw new FormatException($"Invalid purchase record: expected 6 fields, but received {values?.Length ?? 0}.");

            Id = int.Parse(values[0], CultureInfo.InvariantCulture);
            VisitorId = int.Parse(values[1], CultureInfo.InvariantCulture);
            BookIsbn = values[2];
            PurchaseDate = DateTime.ParseExact(values[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Rating = string.IsNullOrWhiteSpace(values[4])
                ? (int?)null
                : int.Parse(values[4], CultureInfo.InvariantCulture);
            Comment = values[5];
        }
    }
}
