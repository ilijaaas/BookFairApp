using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class VisitorsDAO
    {
        private readonly List<Visitor> _visitors;
        private readonly Storage<Visitor> _storage;

        public VisitorsDAO()
        {
            _storage = new Storage<Visitor>("visitors.txt");
            _visitors = _storage.Load();
        }

        private int GenerateId()
        {
            if (_visitors.Count == 0) return 1;
            return _visitors.Max(v => v.Id) + 1;
        }

        public Visitor? AddVisitor(Visitor visitor)
        {
            if (visitor == null) return null;

            if (_visitors.Any(v => v.MembershipCardNumber == visitor.MembershipCardNumber))
                return null;

            bool addressExists = new Storage<Address>("addresses.txt").Load()
                .Any(a => a.Id == visitor.AddressId);
            if (!addressExists) return null;

            var knownBookIsbns = new Storage<Book>("books.txt").Load().Select(b => b.Isbn).ToHashSet();
            var wishlistIsbns = visitor.Wishlist != null && visitor.Wishlist.Count > 0
                ? visitor.Wishlist.Where(b => b != null).Select(b => b.Isbn).ToList()
                : (visitor.WishlistBookIsbns ?? new List<string>());
            if (wishlistIsbns.Any(isbn => !knownBookIsbns.Contains(isbn)))
                return null;

            visitor.Id = GenerateId();
            _visitors.Add(visitor);
            _storage.Save(_visitors);
            return visitor;
        }

        public Visitor? UpdateVisitor(Visitor visitor)
        {
            Visitor? oldVisitor = _visitors.Find(v => v.Id == visitor.Id);
            if (oldVisitor is null) return null;
            if (!new Storage<Address>("addresses.txt").Load().Any(a => a.Id == visitor.AddressId))
                return null;
            if (_visitors.Any(v => v.Id != visitor.Id && v.MembershipCardNumber == visitor.MembershipCardNumber))
                return null;

            var knownBookIsbns = new Storage<Book>("books.txt").Load().Select(b => b.Isbn).ToHashSet();
            var wishlistIsbns = visitor.Wishlist != null && visitor.Wishlist.Count > 0
                ? visitor.Wishlist.Where(b => b != null).Select(b => b.Isbn).ToList()
                : (visitor.WishlistBookIsbns ?? new List<string>());
            if (wishlistIsbns.Any(isbn => !knownBookIsbns.Contains(isbn)))
                return null;

            oldVisitor.FirstName = visitor.FirstName;
            oldVisitor.LastName = visitor.LastName;
            oldVisitor.DateOfBirth = visitor.DateOfBirth;
            oldVisitor.AddressId = visitor.AddressId;
            oldVisitor.ContactPhone = visitor.ContactPhone;
            oldVisitor.Email = visitor.Email;
            oldVisitor.MembershipCardNumber = visitor.MembershipCardNumber;
            oldVisitor.CurrentMembershipYear = visitor.CurrentMembershipYear;
            oldVisitor.Status = visitor.Status;
            oldVisitor.Wishlist = visitor.Wishlist ?? new List<Book>();
            oldVisitor.WishlistBookIsbns = visitor.WishlistBookIsbns ?? new List<string>();

            _storage.Save(_visitors);
            return oldVisitor;
        }

        public Visitor? RemoveVisitor(int id)
        {
            Visitor? visitor = _visitors.Find(v => v.Id == id);
            if (visitor == null) return null;
            if (new Storage<Purchase>("purchases.txt").Load().Any(p => p.VisitorId == id))
                return null;

            _visitors.Remove(visitor);
            _storage.Save(_visitors);
            return visitor;
        }

        public Visitor? GetVisitorById(int id)
        {
            Visitor visitor = _visitors.Find(v => v.Id == id);
            if (visitor != null) RelationshipResolver.Resolve(visitor);
            return visitor;
        }

        public List<Visitor> GetAllVisitors()
        {
            foreach (Visitor visitor in _visitors)
                RelationshipResolver.Resolve(visitor);
            return _visitors;
        }

        public List<Visitor> GetAllVisitors(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Visitor>();

IEnumerable<Visitor> visitors = _visitors;

            switch (sortCriteria)
            {
                case "Id":
                    visitors = sortDirection == SortDirection.Ascending ? _visitors.OrderBy(x => x.Id) : _visitors.OrderByDescending(x => x.Id);
                    break;
                case "FirstName":
                    visitors = sortDirection == SortDirection.Ascending ? _visitors.OrderBy(x => x.FirstName) : _visitors.OrderByDescending(x => x.FirstName);
                    break;
                case "LastName":
                    visitors = sortDirection == SortDirection.Ascending ? _visitors.OrderBy(x => x.LastName) : _visitors.OrderByDescending(x => x.LastName);
                    break;
                case "MembershipCardNumber":
                    visitors = sortDirection == SortDirection.Ascending ? _visitors.OrderBy(x => x.MembershipCardNumber) : _visitors.OrderByDescending(x => x.MembershipCardNumber);
                    break;
                default:
                    visitors = _visitors.OrderBy(x => x.Id);
                    break;
            }

           
            visitors = visitors.Skip((page - 1) * pageSize).Take(pageSize);

            List<Visitor> result = visitors.ToList();
            foreach (Visitor visitor in result)
                RelationshipResolver.Resolve(visitor);
            return result;
        }
    }
}