using System;
using System.Collections.Generic;
using System.Text;


namespace Book_Fair_Management.Model
{
    public class Adress
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Adress() { }

        public Adress(string street, string number, string city, string country)
        {
            Street = street;
            Number = number;
            City = city;
            Country = country;
        }

        public override string ToString()
        {
            return $"{Street} {Number}, {City}, {Country}";
        }
    }
}
