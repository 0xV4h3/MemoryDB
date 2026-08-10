using Core;
using Engine.Serialization;
using Engine.Serialization.Binary;

namespace Test.Tests;

public static class Serializers
{
    public static (string name, string extension, IStorageSerializer serializer) Binary
        => ("BinarySerializer", ".bin", new BinarySerializerStrategy());

    public static (string name, string extension, IStorageSerializer serializer) Json
        => ("JsonSerializer", ".json", new JsonSerializerStrategy());
    
    public static (string name, string extension, IStorageSerializer serializer) Xml
        => ("XmlSerializer", ".xml", new XmlSerializerStrategy());
}