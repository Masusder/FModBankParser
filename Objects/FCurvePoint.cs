using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public struct FCurvePoint
{
    public float X;
    public float Y;
    public float Shape;
    public uint Type;

    public FCurvePoint(BinaryReader Ar)
    {
        X = Ar.ReadSingle();
        Y = Ar.ReadSingle();
        Shape = Ar.ReadSingle();
        Type = Ar.ReadUInt32();
    }
}
