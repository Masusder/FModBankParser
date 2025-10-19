using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FCurvePoint
{
    public readonly float X;
    public readonly float Y;
    public readonly float Shape;
    public readonly uint Type;

    public FCurvePoint(BinaryReader Ar)
    {
        X = Ar.ReadSingle();
        Y = Ar.ReadSingle();
        Shape = Ar.ReadSingle();
        Type = Ar.ReadUInt32();
    }
}
