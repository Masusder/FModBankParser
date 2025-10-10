using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Metadata;

public readonly struct FBankInfo
{
    public readonly FModGuid BaseGuid;
    public readonly ulong Hash;
    public readonly int FileVersion;
    public readonly int ExportFlags;

    public FBankInfo(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Hash = Ar.ReadUInt64();
        FileVersion = Ar.ReadInt32();
        ExportFlags = Ar.ReadInt32();
    }
}
