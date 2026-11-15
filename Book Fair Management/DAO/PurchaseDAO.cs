using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class PurchasesDAO
    {
        private readonly List<Purchase> _purchases;
        private readonly Storage<Purchase> _storage;

        public PurchasesDAO()
        {
            _storage = new Storage<Purchase>("purchases.txt");
            _purchases = _storage.Load();
        }

        private int GenerateId()
        {
            if (_purchases.Count == 0) return 1;
            return _purchases.Max(p => p.Id) + 1;
        }

        public Purchase? AddPurchase(Purchase purchase)
        {
            if (purchase == null) return null;
            bool visitorExists = new Storage<Visitor>("visitors.txt").Load().Any(v => v.Id == purchase.VisitorId);
            bool bookExists = new Storage<Book>("books.txt").Load().Any(b => b.Isbn == purchase.BookIsbn);
            if (!visitorExists || !bookExists) return null;

            purchase.Id = GenerateId();
            _purchases.Add(purchase);
            _storage.Save(_purchases);
            return purchase;
        }

        public Purchase? UpdatePurchase(Purchase purchase)
        {
            if (purchase == null) return null;
            Purchase? oldPurchase = _purchases.Find(p => p.Id == purchase.Id);
            if (oldPurchase is null) return null;

            bool visitorExists = new Storage<Visitor>("visitors.txt").Load().Any(v => v.Id == purchase.VisitorId);
            bool bookExists = new Storage<Book>("books.txt").Load().Any(b => b.Isbn == purchase.BookIsbn);
            if (!visitorExists || !bookExists) return null;

            oldPurchase.VisitorId = purchase.VisitorId;
            oldPurchase.BookIsbn = purchase.BookIsbn;
            oldPurchase.PurchaseDate = purchase.PurchaseDate;
            oldPurchase.Rating = purchase.Rating;
            oldPurchase.Comment = purchase.Comment;

            _storage.Save(_purchases);
            return oldPurchase;
        }

        public Purchase? RemovePurchase(int id)
        {
            Purchase? purchase = GetPurchaseById(id);
            if (purchase == null) return null;

            _purchases.Remove(purchase);
            _storage.Save(_purchases);
            return purchase;
        }

        public Purchase? GetPurchaseById(int id)
        {
            Purchase purchase = _purchases.Find(p => p.Id == id);
            if (purchase != null) RelationshipResolver.Resolve(purchase);
            return purchase;
        }

        public List<Purchase> GetAllPurchases()
        {
            foreach (Purchase purchase in _purchases)
                RelationshipResolver.Resolve(purchase);
            return _purchases;
        }

        public List<Purchase> GetAllPurchases(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Purchase>();

IEnumerable<Purchase> purchases = _purchases;

            switch (sortCriteria)
            {
                case "Id":
                    purchases = sortDirection == SortDirection.Ascending ? _purchases.OrderBy(x => x.Id) : _purchases.OrderByDescending(x => x.Id);
                    break;
                case "PurchaseDate":
                    purchases = sortDirection == SortDirection.Ascending ? _purchases.OrderBy(x => x.PurchaseDate) : _purchases.OrderByDescending(x => x.PurchaseDate);
                    break;
                case "VisitorId":
                    purchases = sortDirection == SortDirection.Ascending ? _purchases.OrderBy(x => x.VisitorId) : _purchases.OrderByDescending(x => x.VisitorId);
                    break;
                case "Rating":
                    purchases = sortDirection == SortDirection.Ascending ? _purchases.OrderBy(x => x.Rating) : _purchases.OrderByDescending(x => x.Rating);
                    break;
                default:
                    purchases = _purchases.OrderBy(x => x.Id);
                    break;
            }

       
            purchases = purchases.Skip((page - 1) * pageSize).Take(pageSize);

            List<Purchase> result = purchases.ToList();
            foreach (Purchase item in result)
                RelationshipResolver.Resolve(item);
            return result;
        }
    }
}