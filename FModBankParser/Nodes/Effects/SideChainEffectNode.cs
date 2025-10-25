using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Effects;

public class SideChainEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly bool IsActive;
    public readonly FModGuid[] Targets;
    public readonly uint InputChannelLayout;
    public readonly FModGuid[] Modulators = [];
    public readonly float SideChainLevel;

    public SideChainEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        IsActive = Ar.ReadBoolean();
        Targets = FModReader.ReadElemListImp<FModGuid>(Ar);
        if (FModReader.Version >= 0x4A && FModReader.Version <= 0x5A) InputChannelLayout = Ar.ReadUInt32();
        if (FModReader.Version >= 0x53) Modulators = FModReader.ReadElemListImp<FModGuid>(Ar);
        if (FModReader.Version >= 0x88) SideChainLevel = Ar.ReadSingle();
    }
}
