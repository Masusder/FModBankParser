using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FRangeFloat
{
    public readonly float Minimum;
    public readonly float Maximum;

    public FRangeFloat(BinaryReader Ar)
    {
        Minimum = Ar.ReadSingle();
        Maximum = Ar.ReadSingle();
    }
}
