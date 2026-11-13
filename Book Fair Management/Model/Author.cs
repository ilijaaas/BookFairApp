using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Model
{
    public class Author
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Adress Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IdCardNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

        public Author() { }

        public Author(string firstName, string lastName, DateTime dateOfBirth, Adress address,
                      string phone, string email, string idCardNumber, int yearsOfExperience)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Address = address;
            Phone = phone;
            Email = email;
            IdCardNumber = idCardNumber;
            YearsOfExperience = yearsOfExperience;
        }
    }
}
