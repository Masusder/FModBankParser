using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.ModulatorSubnodes;

public class SpectralSidechainModulatorNode
{
    public readonly float Amount;
    public readonly ESpectralSidechainModulatorMode Mode;
    public readonly float ThresholdMinimum;
    public readonly float ThresholdMaximum;
    public readonly float AttackTime;
    public readonly float ReleaseTime;
    public readonly FModGuid ThresholdMapping;

    public SpectralSidechainModulatorNode(BinaryReader Ar)
    {
        Amount = Ar.ReadSingle();
        Mode = (ESpectralSidechainModulatorMode)Ar.ReadInt32();
        ThresholdMinimum = Ar.ReadSingle();
        ThresholdMaximum = Ar.ReadSingle();
        AttackTime = Ar.ReadSingle();
        ReleaseTime = Ar.ReadSingle();

        ThresholdMapping = new FModGuid(Ar);
    }
}
