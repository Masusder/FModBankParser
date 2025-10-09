using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class BuiltInEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly EDSPType DSPType;

    public BuiltInEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        DSPType = (EDSPType)Ar.ReadUInt32();
    }
}
