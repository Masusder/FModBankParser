using System.IO;

namespace FModBankParser.Metadata;

public readonly struct HashData
{
    public readonly FHashInfo[] Hashes;

    public HashData(BinaryReader Ar)
    {
        Hashes = FModReader.ReadElemListImp<FHashInfo>(Ar);
    }

    public static implicit operator FHashInfo[](HashData data) => data.Hashes;
}
