using Engine.Serialization.Binary.Compression;
using Engine.Serialization.Binary.Checksum;
    
namespace Engine.Serialization.Binary.Metadata;

internal readonly record struct BinaryFormatHeader(
    int FormatVersion,
    CompressionKind Compression,
    ChecksumAlgorithm ChecksumAlgorithm,
    int UncompressedLength,
    int CompressedLength,
    byte[] Checksum)
{
    public const int Magic = 0x4244424D;

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(Magic);
        writer.Write(FormatVersion);
        writer.Write((byte)Compression);
        writer.Write((byte)ChecksumAlgorithm);
        writer.Write(UncompressedLength);
        writer.Write(CompressedLength);
        writer.Write((byte)Checksum.Length);
        writer.Write(Checksum);
    }

    public static BinaryFormatHeader ReadFrom(BinaryReader reader)
    {
        int magic = reader.ReadInt32();
        if (magic != Magic)
            throw new InvalidDataException("Not a recognized MemoryDB binary stream (magic number mismatch).");

        int formatVersion = reader.ReadInt32();
        var compression = (CompressionKind)reader.ReadByte();
        var checksumAlgorithm = (ChecksumAlgorithm)reader.ReadByte();
        int uncompressedLength = reader.ReadInt32();
        int compressedLength = reader.ReadInt32();
        byte checksumLength = reader.ReadByte();
        byte[] checksum = reader.ReadBytes(checksumLength);

        return new BinaryFormatHeader(formatVersion, compression, checksumAlgorithm, uncompressedLength, compressedLength, checksum);
    }
}