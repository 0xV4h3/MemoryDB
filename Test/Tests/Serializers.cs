using Core;
using Engine.Serialization;
using Engine.Serialization.Binary;
using Engine.Serialization.Binary.Compression;
using Engine.Serialization.Binary.Checksum;

namespace Test.Tests;

public static class Serializers
{
    public static (string name, string extension, IStorageSerializer serializer) DefaultBinary
        => ("BinarySerializer", ".bin", new BinarySerializerStrategy());
    
    public static (string name, string extension, IStorageSerializer serializer) Binary(
        ICompressionStrategy? compressionStrategy = null,
        IIntegrityChecksum? checksum = null)
    {
        var compressor = new Compressor(compressionStrategy ?? new NoCompression());
        var checksumProvider = new ChecksumProvider(checksum ?? new Crc32Checksum());
        var serializer = new BinarySerializerStrategy(compressor, checksumProvider);
        return ("BinarySerializer", ".bin", serializer);
    }
    
    public static (string name, string extension, IStorageSerializer serializer) Json
        => ("JsonSerializer", ".json", new JsonSerializerStrategy());
    
    public static (string name, string extension, IStorageSerializer serializer) Xml
        => ("XmlSerializer", ".xml", new XmlSerializerStrategy());
}