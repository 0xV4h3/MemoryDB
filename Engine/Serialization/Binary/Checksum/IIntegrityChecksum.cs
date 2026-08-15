namespace Engine.Serialization.Binary.Checksum;

public interface IIntegrityChecksum
{
    ChecksumAlgorithm Kind { get; }
    
    byte[] Compute(ReadOnlySpan<byte> data);
}