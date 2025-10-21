using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FMappingPoint
{
    public readonly float X;
    public readonly float Y;

    public FMappingPoint(BinaryReader Ar)
    {
        X = Ar.ReadSingle();
        Y = Ar.ReadSingle();
    }
}