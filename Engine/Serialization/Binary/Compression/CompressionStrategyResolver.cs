namespace Engine.Serialization.Binary.Compression;

internal static class CompressionStrategyResolver
{
    public static ICompressionStrategy Resolve(CompressionKind kind) => kind switch
    {
        CompressionKind.None => new NoCompression(),
        CompressionKind.Deflate => new DeflateCompression(),
        CompressionKind.Brotli => new BrotliCompression(),
        _ => throw new NotSupportedException($"Unknown compression kind in file header: {kind}")
    };
}