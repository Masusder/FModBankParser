using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class ControllerNode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid PropertyOwnerGuid;
    public readonly FModGuid CurveGuid;
    public readonly int PropertyIndex;

    public ControllerNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        PropertyOwnerGuid = new FModGuid(Ar);
        if (FModReader.Version < 0x5a) new FModGuid(Ar);
        CurveGuid = new FModGuid(Ar);
        PropertyIndex = Ar.ReadInt32();
    }
}
