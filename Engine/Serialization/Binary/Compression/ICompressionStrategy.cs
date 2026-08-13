namespace Engine.Serialization.Binary.Compression;

public interface ICompressionStrategy
{
    CompressionKind Kind { get; }
    Stream Wrap(Stream destination);
    Stream Unwrap(Stream source);
}