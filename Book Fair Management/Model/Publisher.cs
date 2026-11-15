using System;
using System.Collections.Generic;
using System.Globalization;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Publisher : ISerializable
    {
        private Author _headOfPublisher;

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int HeadOfPublisherId { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Book> Books { get; set; } = new List<Book>();

        public Author HeadOfPublisher
        {
            get => _headOfPublisher;
            set
            {
                if (value != null && value.YearsOfExperience < 5)
                    throw new InvalidOperationException("The head of the publisher must have at least 5 years of experience.");

                _headOfPublisher = value;
                if (value != null)
                    HeadOfPublisherId = value.Id;
            }
        }

        public Publisher() { }

        public Publisher(string code, string name, Author headOfPublisher)
        {
            Code = code;
            Name = name;
            HeadOfPublisher = headOfPublisher;
        }

        public override string ToString() => $"{Name} (Code: {Code})";

        public string[] ToCSV() => new[]
        {
            Code ?? string.Empty,
            Name ?? string.Empty,
            HeadOfPublisherId.ToString(CultureInfo.InvariantCulture)
        };

        public void FromCSV(string[] values)
        {
            if (values == null || values.Length != 3)
                throw new FormatException($"Invalid publisher record: expected 3 fields, but received. {values?.Length ?? 0}.");

            Code = values[0];
            Name = values[1];
            HeadOfPublisherId = int.Parse(values[2], CultureInfo.InvariantCulture);
        }
    }
}
