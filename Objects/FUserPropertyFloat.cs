using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FUserPropertyFloat
{
    public readonly string Name;
    public readonly float Value;

    public FUserPropertyFloat(BinaryReader Ar)
    {
        Name = FModReader.ReadSerializedString(Ar);
        Value = Ar.ReadSingle();
    }
}