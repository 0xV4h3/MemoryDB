namespace Core;

public interface IStorageSerializer
{
    byte[] Serialize<T>(T data) where T : class;
    
    T? Deserialize<T>(byte[] bytes) where T : class;
    
    string SupportedExtension { get; }
}