namespace Engine.Serialization.Binary.Compression;

public sealed class Compressor(ICompressionStrategy defaultStrategy) : ICompressor
{
    private readonly ICompressionStrategy _defaultStrategy = defaultStrategy;

    public CompressionKind DefaultKind => _defaultStrategy.Kind;

    public byte[] Compress(byte[] rawPayload)
    {
        if (_defaultStrategy.Kind == CompressionKind.None) return rawPayload;

        using var output = new MemoryStream();
        using (var compressingStream = _defaultStrategy.Wrap(output))
            compressingStream.Write(rawPayload, 0, rawPayload.Length);

        return output.ToArray();
    }

    public byte[] Decompress(CompressionKind kind, byte[] compressedPayload, int uncompressedLength)
    {
        if (kind == CompressionKind.None) return compressedPayload;

        var strategy = CompressionStrategyResolver.Resolve(kind);

        using var input = new MemoryStream(compressedPayload);
        using var decompressingStream = strategy.Unwrap(input);

        var output = new byte[uncompressedLength];
        int totalRead = 0;

        while (totalRead < uncompressedLength)
        {
            int read = decompressingStream.Read(output, totalRead, uncompressedLength - totalRead);
            if (read == 0) break;
            totalRead += read;
        }

        if (totalRead != uncompressedLength)
            throw new InvalidDataException(
                $"Decompression ended early. Expected {uncompressedLength} bytes, got {totalRead}.");

        return output;
    }
}