using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public struct FMappingPoint
{
    public float X;
    public float Y;

    public FMappingPoint(BinaryReader Ar)
    {
        X = Ar.ReadSingle();
        Y = Ar.ReadSingle();
    }
}