namespace Engine.Serialization.Binary.Compression;

public enum CompressionKind : byte
{
    None = 0,
    Deflate = 1,
    Brotli = 2,
}