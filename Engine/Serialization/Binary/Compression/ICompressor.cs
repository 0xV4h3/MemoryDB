namespace Engine.Serialization.Binary.Compression;

public interface ICompressor
{
    CompressionKind DefaultKind { get; }

    byte[] Compress(byte[] rawPayload);
    byte[] Decompress(CompressionKind kind, byte[] compressedPayload, int uncompressedLength);
}