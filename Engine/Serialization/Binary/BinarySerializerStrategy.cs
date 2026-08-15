using System.Text;
using Core;
using Engine.Serialization.Binary.Codec;
using Engine.Serialization.Binary.Compression;
using Engine.Serialization.Binary.Checksum;
using Engine.Serialization.Binary.Metadata;

namespace Engine.Serialization.Binary;

public sealed class BinarySerializerStrategy(
    ICompressor? compressor = null,
    IChecksumProvider? checksum = null) : IStorageSerializer
{
    public const int CurrentFormatVersion = 1;

    private readonly ICompressor _compressor = compressor ?? new Compressor(new NoCompression());
    private readonly IChecksumProvider _checksum = checksum ?? new ChecksumProvider(new Crc32Checksum());
    
    public byte[] Serialize<T>(T data) where T : class
    {
        using var ms = new MemoryStream();
        Serialize(ms, data);
        return ms.ToArray();
    }

    public T? Deserialize<T>(byte[] bytes) where T : class
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0) return null;

        using var ms = new MemoryStream(bytes);
        return Deserialize<T>(ms);
    }
    
    public void Serialize<T>(Stream destination, T data) where T : class
    {
        ArgumentNullException.ThrowIfNull(destination);

        byte[] rawPayload = BinaryPayloadCodec.Serialize(data);
        byte[] checksumBytes = _checksum.Compute(rawPayload);
        byte[] compressedPayload = _compressor.Compress(rawPayload);

        var header = new BinaryFormatHeader(
            CurrentFormatVersion, _compressor.DefaultKind, _checksum.DefaultKind,
            rawPayload.Length, compressedPayload.Length, checksumBytes);

        using var writer = new BinaryWriter(destination, Encoding.UTF8, leaveOpen: true);
        header.WriteTo(writer);
        writer.Write(compressedPayload);
        writer.Flush();
    }

    public T? Deserialize<T>(Stream source) where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        using var reader = new BinaryReader(source, Encoding.UTF8, leaveOpen: true);
        var header = BinaryFormatHeader.ReadFrom(reader);

        if (header.FormatVersion != CurrentFormatVersion)
            throw new NotSupportedException(
                $"Binary format version {header.FormatVersion} is not supported by this reader " +
                $"(supports version {CurrentFormatVersion}).");

        byte[] compressedPayload = reader.ReadBytes(header.CompressedLength);
        byte[] rawPayload = _compressor.Decompress(header.Compression, compressedPayload, header.UncompressedLength);

        _checksum.Verify(header.ChecksumAlgorithm, rawPayload, header.Checksum);

        return BinaryPayloadCodec.Deserialize<T>(rawPayload);
    }
}