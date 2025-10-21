using FModBankParser.Objects;

namespace FModBankParser.Metadata;

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
