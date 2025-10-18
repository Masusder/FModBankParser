using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class BuiltInEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly uint InputChannelLayout;
    public readonly EDSPType DSPType;
    public ParameterizedEffectNode? ParamEffectBody;

    public BuiltInEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        if (FModReader.Version < 0x5B) InputChannelLayout = Ar.ReadUInt32();
        DSPType = (EDSPType)Ar.ReadUInt32();

        if (FModReader.Version >= 0x3D && FModReader.Version <= 0x91)
        {
            bool legacyBypass = Ar.ReadBoolean();
        }
    }
}
