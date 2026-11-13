using System;
using System.Collections.Generic;
using System.Text;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Publisher
    {
        private Author _headOfPublisher;

        public string Code { get; set; }
        public string Name { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Book> Books { get; set; } = new List<Book>();

        public Author HeadOfPublisher
        {
            get => _headOfPublisher;
            set
            {
                if (value != null && value.YearsOfExperience < 5)
                {
                    throw new InvalidOperationException("The head of the publisher must have at least 5 years of experience.");
                }
                _headOfPublisher = value;
            }
        }

        public Publisher() { }

        public Publisher(string code, string name, Author headOfPublisher)
        {
            Code = code;
            Name = name;
            HeadOfPublisher = headOfPublisher;
        }
    }
}
