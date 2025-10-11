using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class SideChainEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly bool IsActive;
    public readonly FModGuid[] Targets;
    public readonly FModGuid[] Modulators;
    public readonly float SideChainLevel;

    public SideChainEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        IsActive = Ar.ReadBoolean();
        Targets = FModReader.ReadElemListImp<FModGuid>(Ar);
        Modulators = FModReader.ReadElemListImp<FModGuid>(Ar);
        SideChainLevel = Ar.ReadSingle();
    }
}
