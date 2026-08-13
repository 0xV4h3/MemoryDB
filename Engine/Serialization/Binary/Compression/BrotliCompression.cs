using System.IO.Compression;

namespace Engine.Serialization.Binary.Compression;

public sealed class BrotliCompression(CompressionLevel level = CompressionLevel.Optimal) : ICompressionStrategy
{
    public CompressionKind Kind => CompressionKind.Brotli;
    public Stream Wrap(Stream destination) => new BrotliStream(destination, level, leaveOpen: true);
    public Stream Unwrap(Stream source) => new BrotliStream(source, CompressionMode.Decompress, leaveOpen: true);
}