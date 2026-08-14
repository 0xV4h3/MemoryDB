using Core;
using Engine.Serialization;
using Engine.Serialization.Binary;
using Engine.Serialization.Binary.Compression;

namespace Test.Tests;

public static class Serializers
{
    public static (string name, string extension, IStorageSerializer serializer) DefaultBinary
        => ("BinarySerializer", ".bin", new BinarySerializerStrategy(new Compressor(new NoCompression())));
    
    public static (string name, string extension, IStorageSerializer serializer) Binary(
        ICompressionStrategy compressionStrategy)
    {
        var compressor = new Compressor(compressionStrategy);
        var serializer = new BinarySerializerStrategy(compressor);
        return ("BinarySerializer", ".bin", serializer);
    }
    
    public static (string name, string extension, IStorageSerializer serializer) Json
        => ("JsonSerializer", ".json", new JsonSerializerStrategy());
    
    public static (string name, string extension, IStorageSerializer serializer) Xml
        => ("XmlSerializer", ".xml", new XmlSerializerStrategy());
}