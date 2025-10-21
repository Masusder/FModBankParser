namespace FModBankParser.Objects;

public readonly struct FPlaylistEntry
{
    public readonly FModGuid Guid;
    public readonly float Weight;

    public FPlaylistEntry(BinaryReader Ar)
    {
        Guid = new FModGuid(Ar);
        Weight = Ar.ReadSingle();
    }
}
