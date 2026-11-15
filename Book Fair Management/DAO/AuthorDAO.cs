using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class AuthorsDAO
    {
        private readonly List<Author> _authors;
        private readonly Storage<Author> _storage;

        public AuthorsDAO()
        {
            _storage = new Storage<Author>("authors.txt");
            _authors = _storage.Load();

            
            var addressStorage = new Storage<Address>("addresses.txt");
            var addresses = addressStorage.Load();
            bool migrated = false;
            foreach (Author author in _authors.Where(a => a.NeedsAddressMigration))
            {
                Address existing = addresses.FirstOrDefault(a =>
                    a.Street == author.Address.Street &&
                    a.Number == author.Address.Number &&
                    a.City == author.Address.City &&
                    a.Country == author.Address.Country);

                if (existing == null)
                {
                    int newId = addresses.Count == 0 ? 1 : addresses.Max(a => a.Id) + 1;
                    existing = new Address(newId, author.Address.Street, author.Address.Number,
                                            author.Address.City, author.Address.Country);
                    addresses.Add(existing);
                }

                author.AddressId = existing.Id;
                migrated = true;
            }

            if (migrated)
            {
                addressStorage.Save(addresses);
                _storage.Save(_authors);
            }
        }

        private int GenerateId()
        {
            if (_authors.Count == 0) return 1;
            return _authors.Max(a => a.Id) + 1;
        }

        public Author? AddAuthor(Author author)
        {
            if (author == null) return null;
            if (_authors.Any(a => a.IdCardNumber == author.IdCardNumber)) return null;
            if (!new Storage<Address>("addresses.txt").Load().Any(a => a.Id == author.AddressId))
                return null;

            author.Id = GenerateId();
            _authors.Add(author);
            _storage.Save(_authors);
            return author;
        }

        public Author? UpdateAuthor(Author author)
        {
            if (author == null) return null;
            Author? oldAuthor = _authors.Find(a => a.Id == author.Id);
            if (oldAuthor is null) return null;
            if (!new Storage<Address>("addresses.txt").Load().Any(a => a.Id == author.AddressId))
                return null;
            if (_authors.Any(a => a.Id != author.Id && a.IdCardNumber == author.IdCardNumber))
                return null;
            if (author.YearsOfExperience < 5 &&
                new Storage<Publisher>("publishers.txt").Load().Any(pub => pub.HeadOfPublisherId == author.Id))
                return null;

            oldAuthor.FirstName = author.FirstName;
            oldAuthor.LastName = author.LastName;
            oldAuthor.DateOfBirth = author.DateOfBirth;
            oldAuthor.AddressId = author.AddressId;
            oldAuthor.Address = null;
            oldAuthor.Phone = author.Phone;
            oldAuthor.Email = author.Email;
            oldAuthor.IdCardNumber = author.IdCardNumber;
            oldAuthor.YearsOfExperience = author.YearsOfExperience;

            _storage.Save(_authors);
            return oldAuthor;
        }

        public Author? RemoveAuthor(int id)
        {
            Author? author = _authors.Find(a => a.Id == id);
            if (author == null) return null;
            bool usedByBook = new Storage<Book>("books.txt").Load().Any(b => b.AuthorIds.Contains(id));
            bool isPublisherHead = new Storage<Publisher>("publishers.txt").Load().Any(p => p.HeadOfPublisherId == id);
            if (usedByBook || isPublisherHead) return null;

            _authors.Remove(author);
            _storage.Save(_authors);
            return author;
        }

        public Author? GetAuthorById(int id)
        {
            Author author = _authors.Find(a => a.Id == id);
            if (author != null) RelationshipResolver.Resolve(author);
            return author;
        }

        public List<Author> GetAllAuthors()
        {
            foreach (Author author in _authors)
                RelationshipResolver.Resolve(author);
            return _authors;
        }

        public List<Author> GetAllAuthors(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Author>();

IEnumerable<Author> authors = _authors;

            switch (sortCriteria)
            {
                case "Id":
                    authors = sortDirection == SortDirection.Ascending ? _authors.OrderBy(x => x.Id) : _authors.OrderByDescending(x => x.Id);
                    break;
                case "FirstName":
                    authors = sortDirection == SortDirection.Ascending ? _authors.OrderBy(x => x.FirstName) : _authors.OrderByDescending(x => x.FirstName);
                    break;
                case "LastName":
                    authors = sortDirection == SortDirection.Ascending ? _authors.OrderBy(x => x.LastName) : _authors.OrderByDescending(x => x.LastName);
                    break;
                case "YearsOfExperience":
                    authors = sortDirection == SortDirection.Ascending ? _authors.OrderBy(x => x.YearsOfExperience) : _authors.OrderByDescending(x => x.YearsOfExperience);
                    break;
                default:
                    authors = _authors.OrderBy(x => x.Id);
                    break;
            }

            
            authors = authors.Skip((page - 1) * pageSize).Take(pageSize);

            List<Author> result = authors.ToList();
            foreach (Author item in result)
                RelationshipResolver.Resolve(item);
            return result;
        }
    }
}