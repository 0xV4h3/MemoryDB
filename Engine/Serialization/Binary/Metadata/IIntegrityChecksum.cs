namespace Engine.Serialization.Binary.Metadata;

public interface IIntegrityChecksum
{
    uint Compute(ReadOnlySpan<byte> data);
}