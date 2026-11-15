using System;
using System.Collections.Generic;
using System.Text;

namespace Book_Fair_Management.Serialization
{
    public class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public string ToCSV(List<T> objects)
        {
            var builder = new StringBuilder();

            foreach (T obj in objects)
            {
                string[] fields = obj.ToCSV();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (i > 0) builder.Append(Delimiter);
                    builder.Append(Escape(fields[i] ?? string.Empty));
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public List<T> FromCSV(IEnumerable<string> lines)
        {
            var objects = new List<T>();
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    string[] fields = ParseLine(line);
                    T obj = new T();
                    obj.FromCSV(fields);
                    objects.Add(obj);
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentException || ex is OverflowException)
                {
                    throw new FormatException($"Greška u CSV zapisu, linija {lineNumber}: {ex.Message}", ex);
                }
            }

            return objects;
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\")
                        .Replace("|", "\\|")
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n");
        }

        private static string[] ParseLine(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool escaped = false;

            foreach (char ch in line)
            {
                if (escaped)
                {
                    switch (ch)
                    {
                        case '\\': current.Append('\\'); break;
                        case '|': current.Append('|'); break;
                        case 'r': current.Append('\r'); break;
                        case 'n': current.Append('\n'); break;
                        default:
                            // Preserve unknown escape sequences rather than silently losing data.
                            current.Append('\\').Append(ch);
                            break;
                    }
                    escaped = false;
                }
                else if (ch == '\\')
                {
                    escaped = true;
                }
                else if (ch == Delimiter)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(ch);
                }
            }

            if (escaped) current.Append('\\');
            fields.Add(current.ToString());
            return fields.ToArray();
        }
    }
}
