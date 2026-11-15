using System;
using System.Globalization;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model;

public class Address : ISerializable
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public Address() { }

    public Address(int id, string street, string number, string city, string country)
    {
        Id = id;
        Street = street;
        Number = number;
        City = city;
        Country = country;
    }

    public override string ToString() => $"{Street} {Number}, {City}, {Country}";

    public string[] ToCSV() => new[]
    {
        Id.ToString(CultureInfo.InvariantCulture),
        Street ?? string.Empty,
        Number ?? string.Empty,
        City ?? string.Empty,
        Country ?? string.Empty
    };

    public void FromCSV(string[] values)
    {
        if (values == null || values.Length != 5)
            throw new FormatException("Invalid address record: expected 5 fields.");

        Id = int.Parse(values[0], CultureInfo.InvariantCulture);
        Street = values[1];
        Number = values[2];
        City = values[3];
        Country = values[4];
    }
}
