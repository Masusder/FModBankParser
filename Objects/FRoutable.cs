using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FRoutable
{
    public readonly FModGuid BaseGuid;
    public readonly uint OutputChannelLayout;
    public readonly uint ChannelMask;

    public FRoutable(BinaryReader Ar)
    {
        Ar.ReadInt16(); // Payload size
        BaseGuid = new FModGuid(Ar);
        OutputChannelLayout = Ar.ReadUInt32();
        ChannelMask = Ar.ReadUInt32();
    }
}
