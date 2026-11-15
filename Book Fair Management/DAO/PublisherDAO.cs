using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class PublishersDAO
    {
        private readonly List<Publisher> _publishers;
        private readonly Storage<Publisher> _storage;

        public PublishersDAO()
        {
            _storage = new Storage<Publisher>("publishers.txt");
            _publishers = _storage.Load();
        }

        public Publisher? AddPublisher(Publisher publisher)
        {
            if (publisher == null || _publishers.Any(p => p.Code == publisher.Code))
                return null;

            Author head = new Storage<Author>("authors.txt").Load()
                .FirstOrDefault(a => a.Id == publisher.HeadOfPublisherId);
            if (head == null || head.YearsOfExperience < 5)
                return null;

            publisher.HeadOfPublisher = head;
            _publishers.Add(publisher);
            _storage.Save(_publishers);
            return publisher;
        }

        public Publisher? UpdatePublisher(Publisher publisher)
        {
            if (publisher == null) return null;
            Publisher? oldPublisher = _publishers.Find(p => p.Code == publisher.Code);
            if (oldPublisher is null) return null;

            Author head = new Storage<Author>("authors.txt").Load()
                .FirstOrDefault(a => a.Id == publisher.HeadOfPublisherId);
            if (head == null || head.YearsOfExperience < 5)
                return null;

            oldPublisher.Name = publisher.Name;
            oldPublisher.HeadOfPublisherId = publisher.HeadOfPublisherId;
            oldPublisher.HeadOfPublisher = head;

            _storage.Save(_publishers);
            return oldPublisher;
        }

        public Publisher? RemovePublisher(string code)
        {
            Publisher? publisher = _publishers.Find(p => p.Code == code);
            if (publisher == null) return null;
            if (new Storage<Book>("books.txt").Load().Any(b => b.PublisherCode == code))
                return null;

            _publishers.Remove(publisher);
            _storage.Save(_publishers);
            return publisher;
        }

        public Publisher? GetPublisherByCode(string code)
        {
            Publisher publisher = _publishers.Find(p => p.Code == code);
            if (publisher != null) RelationshipResolver.Resolve(publisher);
            return publisher;
        }

        public List<Publisher> GetAllPublishers()
        {
            foreach (Publisher publisher in _publishers)
                RelationshipResolver.Resolve(publisher);
            return _publishers;
        }

        public List<Publisher> GetAllPublishers(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Publisher>();

IEnumerable<Publisher> publishers = _publishers;

            switch (sortCriteria)
            {
                case "Code":
                    publishers = sortDirection == SortDirection.Ascending ? _publishers.OrderBy(x => x.Code) : _publishers.OrderByDescending(x => x.Code);
                    break;
                case "Name":
                    publishers = sortDirection == SortDirection.Ascending ? _publishers.OrderBy(x => x.Name) : _publishers.OrderByDescending(x => x.Name);
                    break;
                case "HeadOfPublisherId":
                    publishers = sortDirection == SortDirection.Ascending ? _publishers.OrderBy(x => x.HeadOfPublisherId) : _publishers.OrderByDescending(x => x.HeadOfPublisherId);
                    break;
                default:
                    publishers = _publishers.OrderBy(x => x.Code);
                    break;
            }

        
            publishers = publishers.Skip((page - 1) * pageSize).Take(pageSize);

            List<Publisher> result = publishers.ToList();
            foreach (Publisher item in result)
                RelationshipResolver.Resolve(item);
            return result;
        }
    }
}