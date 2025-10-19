using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FLegacyTriggerBox
{
    public readonly FModGuid InstrumentGuid;
    public readonly float Position;
    public readonly float Length;

    public FLegacyTriggerBox(BinaryReader Ar)
    {
        InstrumentGuid = new FModGuid(Ar);
        Position = Ar.ReadSingle();
        Length = Ar.ReadSingle();
    }
}
