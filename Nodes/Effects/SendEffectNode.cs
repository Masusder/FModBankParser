using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class SendEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly uint InputChannelLayout;
    public readonly FModGuid ReturnGuid;
    public readonly float SendLevel;

    public SendEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        if (FModReader.Version < 0x5B) InputChannelLayout = Ar.ReadUInt32();
        ReturnGuid = new FModGuid(Ar);
        SendLevel = Ar.ReadSingle();

        if (FModReader.Version >= 0x3D && FModReader.Version <= 0x91)
        {
            bool legacyBypass = Ar.ReadBoolean();
        }
    }
}
