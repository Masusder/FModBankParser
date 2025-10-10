using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Metadata;

public readonly struct FFormatInfo
{
    public readonly int FileVersion;
    public readonly int CompatVersion;

    public FFormatInfo(BinaryReader Ar)
    {
        FileVersion = Ar.ReadInt32();
        CompatVersion = Ar.ReadInt32();
    }
}
