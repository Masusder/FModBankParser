using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class FormatInfo
{
    public readonly int FileVersion;
    public readonly int CompatVersion;

    public FormatInfo(BinaryReader Ar)
    {
        FileVersion = Ar.ReadInt32();
        CompatVersion = Ar.ReadInt32();
    }
}
