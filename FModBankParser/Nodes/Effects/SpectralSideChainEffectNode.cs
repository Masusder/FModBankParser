using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Effects;

public class SpectralSideChainEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly float Level;
    public readonly float MinimumFrequency;
    public readonly float MaximumFrequency;
    public readonly uint Flags;
    public readonly FModGuid[] Targets;

    public SpectralSideChainEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Level = Ar.ReadSingle();
        MinimumFrequency = Ar.ReadSingle();
        MaximumFrequency = Ar.ReadSingle();
        Flags = Ar.ReadUInt32();
        Ar.ReadBytes(8);
        Targets = FModReader.ReadElemListImp<FModGuid>(Ar);
    }
}
