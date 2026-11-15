using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Visitor : ISerializable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ContactPhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MembershipCardNumber { get; set; } = string.Empty;
        public int CurrentMembershipYear { get; set; }
        public VisitorStatus Status { get; set; } = VisitorStatus.Regular;
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Book> Wishlist { get; set; } = new List<Book>();
        
        public List<string> WishlistBookIsbns { get; set; } = new List<string>();

        public double AverageReviewScore
        {
            get
            {
                var ratings = Purchases.Where(p => p.Rating.HasValue).Select(p => p.Rating.Value);
                return ratings.Any() ? ratings.Average() : 0.0;
            }
        }

        public Visitor() { }

        public Visitor(int id, string firstName, string lastName, DateTime dateOfBirth, int addressId,
                       string contactPhone, string email, string membershipCardNumber,
                       int currentMembershipYear, VisitorStatus status)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            AddressId = addressId;
            ContactPhone = contactPhone;
            Email = email;
            MembershipCardNumber = membershipCardNumber;
            CurrentMembershipYear = currentMembershipYear;
            Status = status;
        }

        public override string ToString() => $"{FirstName} {LastName} (Card: {MembershipCardNumber})";

        public string[] ToCSV() => new[]
        {
            Id.ToString(CultureInfo.InvariantCulture),
            FirstName ?? string.Empty,
            LastName ?? string.Empty,
            DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            AddressId.ToString(CultureInfo.InvariantCulture),
            ContactPhone ?? string.Empty,
            Email ?? string.Empty,
            MembershipCardNumber ?? string.Empty,
            CurrentMembershipYear.ToString(CultureInfo.InvariantCulture),
            Status.ToString(),
            string.Join(",", Wishlist != null && Wishlist.Count > 0
                ? Wishlist.Where(b => b != null && !string.IsNullOrWhiteSpace(b.Isbn)).Select(b => b.Isbn).Distinct()
                : (WishlistBookIsbns ?? new List<string>()).Distinct())
        };

        public void FromCSV(string[] values)
        {
            if (values == null || (values.Length != 10 && values.Length != 11))
                throw new FormatException($"Invalid visitor record: expected 10 or 11 fields, but received {values?.Length ?? 0}.");

            Id = int.Parse(values[0], CultureInfo.InvariantCulture);
            FirstName = values[1];
            LastName = values[2];
            DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            AddressId = int.Parse(values[4], CultureInfo.InvariantCulture);
            ContactPhone = values[5];
            Email = values[6];
            MembershipCardNumber = values[7];
            CurrentMembershipYear = int.Parse(values[8], CultureInfo.InvariantCulture);

            if (!Enum.TryParse(values[9], true, out VisitorStatus status) ||
                !Enum.IsDefined(typeof(VisitorStatus), status))
                throw new FormatException($"Unknown visitor status: '{values[9]}'.");

            Status = status;

            WishlistBookIsbns = values.Length == 11 && !string.IsNullOrWhiteSpace(values[10])
                ? values[10].Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct().ToList()
                : new List<string>();
        }
    }
}
