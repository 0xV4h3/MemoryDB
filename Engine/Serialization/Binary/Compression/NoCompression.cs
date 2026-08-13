namespace Engine.Serialization.Binary.Compression;

public sealed class NoCompression : ICompressionStrategy
{
    public CompressionKind Kind => CompressionKind.None;
    public Stream Wrap(Stream destination) => destination;
    public Stream Unwrap(Stream source) => source;
}