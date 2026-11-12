using System;
using System.Collections.Generic;
using System.Text;

namespace Book_Fair_Management.Model
{
    public class Purchase
    {
        private int? _rating;

        public Visitor Visitor { get; set; }
        public Book Book { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Comment { get; set; }

        public int? Rating
        {
            get => _rating;
            set
            {
                if (value.HasValue && (value < 1 || value > 5))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 5.");
                }
                _rating = value;
            }
        }

        public Purchase() { }

        public Purchase(Visitor visitor, Book book, DateTime purchaseDate, int? rating = null, string comment = null)
        {
            Visitor = visitor;
            Book = book;
            PurchaseDate = purchaseDate;
            Rating = rating;
            Comment = comment;
        }
    }
}
