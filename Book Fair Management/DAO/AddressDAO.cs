using Book_Fair_Management.Model;
using Book_Fair_Management.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Book_Fair_Management.DAO
{
    public class AddressesDAO
    {
        private readonly List<Address> _addresses;
        private readonly Storage<Address> _storage;

        public AddressesDAO()
        {
            _storage = new Storage<Address>("addresses.txt");
            _addresses = _storage.Load();
        }

        private int GenerateId()
        {
            if (_addresses.Count == 0) return 1;
            return _addresses.Max(a => a.Id) + 1;
        }

        public Address? AddAddress(Address address)
        {
            address.Id = GenerateId();
            _addresses.Add(address);
            _storage.Save(_addresses);
            return address;
        }

        public Address? UpdateAddress(Address address)
        {
            Address? oldAddress = GetAddressById(address.Id);
            if (oldAddress is null) return null;

            oldAddress.Street = address.Street;
            oldAddress.Number = address.Number;
            oldAddress.City = address.City;
            oldAddress.Country = address.Country;

            _storage.Save(_addresses);
            return oldAddress;
        }

        public Address? RemoveAddress(int id)
        {
            Address? address = GetAddressById(id);
            if (address == null) return null;

            bool usedByVisitor = new Storage<Visitor>("visitors.txt").Load().Any(v => v.AddressId == id);
            bool usedByAuthor = new Storage<Author>("authors.txt").Load().Any(a => a.AddressId == id);
            if (usedByVisitor || usedByAuthor) return null;

            _addresses.Remove(address);
            _storage.Save(_addresses);
            return address;
        }

        public Address? GetAddressById(int id)
        {
            Address address = _addresses.Find(a => a.Id == id);
            return address;
        }

        public List<Address> GetAllAddresses()
        {
            return _addresses;
        }

        public List<Address> GetAllAddresses(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
                        if (page < 1 || pageSize < 1) return new List<Address>();

IEnumerable<Address> addresses = _addresses;

            switch (sortCriteria)
            {
                case "Id":
                    addresses = sortDirection == SortDirection.Ascending ? _addresses.OrderBy(x => x.Id) : _addresses.OrderByDescending(x => x.Id);
                    break;
                case "City":
                    addresses = sortDirection == SortDirection.Ascending ? _addresses.OrderBy(x => x.City) : _addresses.OrderByDescending(x => x.City);
                    break;
                case "Country":
                    addresses = sortDirection == SortDirection.Ascending ? _addresses.OrderBy(x => x.Country) : _addresses.OrderByDescending(x => x.Country);
                    break;
                case "Street":
                    addresses = sortDirection == SortDirection.Ascending ? _addresses.OrderBy(x => x.Street) : _addresses.OrderByDescending(x => x.Street);
                    break;
                default:
                    addresses = _addresses.OrderBy(x => x.Id);
                    break;
            }

         
            addresses = addresses.Skip((page - 1) * pageSize).Take(pageSize);

            return addresses.ToList();
        }
    }
}