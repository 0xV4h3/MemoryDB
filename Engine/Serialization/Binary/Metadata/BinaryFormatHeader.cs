using Engine.Serialization.Binary.Compression;
    
namespace Engine.Serialization.Binary.Metadata;

internal readonly record struct BinaryFormatHeader(
    int FormatVersion,
    CompressionKind Compression,
    int UncompressedLength,
    int CompressedLength,
    uint Checksum)
{
    public const int Magic = 0x4244424D;

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(Magic);
        writer.Write(FormatVersion);
        writer.Write((byte)Compression);
        writer.Write(UncompressedLength);
        writer.Write(CompressedLength);
        writer.Write(Checksum);
    }

    public static BinaryFormatHeader ReadFrom(BinaryReader reader)
    {
        int magic = reader.ReadInt32();
        if (magic != Magic)
            throw new InvalidDataException("Not a recognized MemoryDB binary stream (magic number mismatch).");

        int formatVersion = reader.ReadInt32();
        var compression = (CompressionKind)reader.ReadByte();
        int uncompressedLength = reader.ReadInt32();
        int compressedLength = reader.ReadInt32();
        uint checksum = reader.ReadUInt32();

        return new BinaryFormatHeader(formatVersion, compression, uncompressedLength, compressedLength, checksum);
    }
}