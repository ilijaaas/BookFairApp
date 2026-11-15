using System;
using System.Collections.Generic;
using System.IO;
using Book_Fair_Management.Serialization;

namespace Book_Fair_Management.Storage
{
    public class Storage<T> where T : ISerializable, new()
    {
        private readonly string _fileName;
        private readonly Serializer<T> _serializer = new Serializer<T>();

        public Storage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Naziv datoteke nije zadat.", nameof(fileName));

            string dataDirectory = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data"));
            Directory.CreateDirectory(dataDirectory);
            _fileName = Path.Combine(dataDirectory, Path.GetFileName(fileName));
            Console.WriteLine(dataDirectory);

        }

        public List<T> Load()
        {
            string directory = Path.GetDirectoryName(_fileName);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(_fileName))
                File.Create(_fileName).Dispose();

            return _serializer.FromCSV(File.ReadLines(_fileName));
        }

        public void Save(List<T> objects)
        {
            string directory = Path.GetDirectoryName(_fileName);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            string serializedObjects = _serializer.ToCSV(objects);
            string temporaryFile = _fileName + ".tmp";

            File.WriteAllText(temporaryFile, serializedObjects);
            File.Move(temporaryFile, _fileName, true);
        }
    }
}
