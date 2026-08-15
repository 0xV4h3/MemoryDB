namespace Engine.Serialization.Binary.Checksum;

public interface IChecksumProvider
{
    ChecksumAlgorithm DefaultKind { get; }
    
    byte[] Compute(byte[] rawPayload);
    void Verify(ChecksumAlgorithm kind, byte[] rawPayload, byte[] expectedChecksum);
}