using FModUEParser.Objects;

namespace FModUEParser.Metadata;

public readonly struct FHashInfo
{
    public readonly FModGuid Guid;
    public readonly uint Hash;

    public FHashInfo(BinaryReader Ar)
    {
        Guid = new FModGuid(Ar);
        Hash = Ar.ReadUInt32();
    }
}
