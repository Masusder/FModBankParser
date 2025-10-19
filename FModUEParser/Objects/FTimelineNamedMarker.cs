using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FTimelineNamedMarker
{
    public readonly FModGuid BaseGuid;
    public readonly uint Position;
    public readonly string Name;
    public readonly uint Length;

    public FTimelineNamedMarker(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Position = Ar.ReadUInt32();
        Name = FModReader.ReadString(Ar);

        if (FModReader.Version >= 0x79)
        {
            Length = Ar.ReadUInt32();
        }
    }
}
