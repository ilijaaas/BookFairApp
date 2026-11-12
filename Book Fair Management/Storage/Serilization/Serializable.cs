namespace Book_Fair_Management.Serialization;

public interface ISerializable
{
    string[] ToCSV();

    void FromCSV(string[] values);
}