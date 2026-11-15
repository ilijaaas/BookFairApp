using System;
using System.Collections.Generic;
using System.Globalization;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Author : ISerializable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IdCardNumber { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

      
        internal bool HasLegacyInlineAddress { get; private set; }

        public Author() { }

        public Author(int id, string firstName, string lastName, DateTime dateOfBirth, int addressId,
                      string phone, string email, string idCardNumber, int yearsOfExperience)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            AddressId = addressId;
            Phone = phone;
            Email = email;
            IdCardNumber = idCardNumber;
            YearsOfExperience = yearsOfExperience;
        }

        
        public Author(int id, string firstName, string lastName, DateTime dateOfBirth, Address address,
                      string phone, string email, string idCardNumber, int yearsOfExperience)
            : this(id, firstName, lastName, dateOfBirth, address?.Id ?? 0, phone, email, idCardNumber, yearsOfExperience)
        {
            Address = address;
        }

        public override string ToString() => $"{FirstName} {LastName} ({Email})";

        public string[] ToCSV() => new[]
        {
            Id.ToString(CultureInfo.InvariantCulture),
            FirstName ?? string.Empty,
            LastName ?? string.Empty,
            DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            AddressId.ToString(CultureInfo.InvariantCulture),
            Phone ?? string.Empty,
            Email ?? string.Empty,
            IdCardNumber ?? string.Empty,
            YearsOfExperience.ToString(CultureInfo.InvariantCulture)
        };

        public void FromCSV(string[] values)
        {
            if (values == null)
                throw new FormatException("Invalid author record.");

            if (values.Length == 9)
            {
                Id = int.Parse(values[0], CultureInfo.InvariantCulture);
                FirstName = values[1];
                LastName = values[2];
                DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                AddressId = int.Parse(values[4], CultureInfo.InvariantCulture);
                Phone = values[5];
                Email = values[6];
                IdCardNumber = values[7];
                YearsOfExperience = int.Parse(values[8], CultureInfo.InvariantCulture);
                Address = null;
                HasLegacyInlineAddress = false;
                return;
            }


            if (values.Length == 12)
            {
                Id = int.Parse(values[0], CultureInfo.InvariantCulture);
                FirstName = values[1];
                LastName = values[2];
                DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                AddressId = 0;
                Address = new Address(0, values[4], values[5], values[6], values[7]);
                Phone = values[8];
                Email = values[9];
                IdCardNumber = values[10];
                YearsOfExperience = int.Parse(values[11], CultureInfo.InvariantCulture);
                HasLegacyInlineAddress = true;
                return;
            }

            throw new FormatException($"Invalid author record: expected 9 fields, but received {values.Length}.");
        }

        internal bool NeedsAddressMigration => HasLegacyInlineAddress && AddressId == 0 && Address != null;
    }
}
