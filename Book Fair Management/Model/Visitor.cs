using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Book_Fair_Management.Model
{
    public class Visitor
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Adress Address { get; set; }
            public string ContactPhone { get; set; }
            public string Email { get; set; }
            public string MembershipCardNumber { get; set; }
            public int CurrentMembershipYear { get; set; }
            public VisitorStatus Status { get; set; }
            public List<Purchase> Purchases { get; set; } = new List<Purchase>();
            public List<Book> Wishlist { get; set; } = new List<Book>();

            public double AverageReviewScore
            {
                get
                {
                    var ratings = Purchases.Where(p => p.Rating.HasValue).Select(p => p.Rating.Value);
                    return ratings.Any() ? ratings.Average() : 0.0;
                }
            }

            public Visitor() { }

            public Visitor(string firstName, string lastName, DateTime dateOfBirth, Adress address,
                           string contactPhone, string email, string membershipCardNumber,
                           int currentMembershipYear, VisitorStatus status)
            {
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                Address = address;
                ContactPhone = contactPhone;
                Email = email;
                MembershipCardNumber = membershipCardNumber;
                CurrentMembershipYear = currentMembershipYear;
                Status = status;
            }
        }
}
