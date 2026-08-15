namespace Engine.Serialization.Binary.Checksum;

internal static class ChecksumResolver
{
    public static IIntegrityChecksum Resolve(ChecksumAlgorithm algorithm) => algorithm switch
    {
        ChecksumAlgorithm.Crc32 => new Crc32Checksum(),
        _ => throw new NotSupportedException($"Unknown checksum algorithm in file header: {algorithm}")
    };
}