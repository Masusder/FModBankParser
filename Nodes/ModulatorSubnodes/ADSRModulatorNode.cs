using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.ModulatorSubnodes;

/// ADSR (Attack, Decay, Sustain, Release) modulator
public class ADSRModulatorNode
{
    public float PeakValue { get; }
    public float SustainValue { get; }
    public float AttackTime { get; }
    public float HoldTime { get; }
    public float DecayTime { get; }
    public float ReleaseTime { get; }
    public float AttackShape { get; }
    public float DecayShape { get; }
    public float ReleaseShape { get; }
    public float? FinalValue { get; }

    public ADSRModulatorNode(BinaryReader Ar)
    {
        PeakValue = Ar.ReadSingle();
        SustainValue = Ar.ReadSingle();
        AttackTime = Ar.ReadSingle();
        HoldTime = Ar.ReadSingle();
        DecayTime = Ar.ReadSingle();
        ReleaseTime = Ar.ReadSingle();
        AttackShape = Ar.ReadSingle();
        DecayShape = Ar.ReadSingle();
        ReleaseShape = Ar.ReadSingle();

        if (FModReader.Version >= 0x74)
        {
            FinalValue = Ar.ReadSingle();
        }
    }
}
