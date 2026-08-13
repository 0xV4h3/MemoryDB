using System.IO.Compression;

namespace Engine.Serialization.Binary.Compression;

public sealed class DeflateCompression(CompressionLevel level = CompressionLevel.Optimal) : ICompressionStrategy
{
    public CompressionKind Kind => CompressionKind.Deflate;
    public Stream Wrap(Stream destination) => new DeflateStream(destination, level, leaveOpen: true);
    public Stream Unwrap(Stream source) => new DeflateStream(source, CompressionMode.Decompress, leaveOpen: true);
}