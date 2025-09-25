namespace FModUEParser.Objects;

public struct FHashData
{
    public readonly FModGuid Guid;
    public readonly uint Hash;

    public FHashData(BinaryReader Ar)
    {
        Guid = new FModGuid(Ar);
        Hash = Ar.ReadUInt32();
    }
}
