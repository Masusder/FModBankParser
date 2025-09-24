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
    public float Amount { get; set; }
    public ESpectralSidechainModulatorMode Mode { get; set; }
    public float ThresholdMinimum { get; set; }
    public float ThresholdMaximum { get; set; }
    public float AttackTime { get; set; }
    public float ReleaseTime { get; set; }
    public FModGuid ThresholdMapping { get; set; }

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
