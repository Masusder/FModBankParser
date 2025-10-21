using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Metadata;

public readonly struct FBankInfo
{
    public readonly FModGuid BaseGuid;
    public readonly ulong Hash;
    public readonly int FileVersion;
    public readonly int TopLevelEventCount;
    public readonly int ExportFlags;

    public FBankInfo(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        if (FModReader.Version >= 0x37) Hash = Ar.ReadUInt64();
        FileVersion = FModReader.Version;
        if (FModReader.Version >= 0x41) TopLevelEventCount = Ar.ReadInt32();
        if (FModReader.Version >= 0x4D) ExportFlags = Ar.ReadInt32();
    }
}
