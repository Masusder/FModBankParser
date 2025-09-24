using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.ModulatorSubnodes;

public class EnvelopeModulatorNode
{
    public float Amount { get; }
    public float ThresholdMinimum { get; }
    public float ThresholdMaximum { get; }
    public float? AttackTime { get; }
    public float? ReleaseTime { get; }
    public bool? UseRMS { get; }

    public float? Minimum { get; }
    public float? Maximum { get; }
    public FModGuid? EffectId { get; }

    public EnvelopeModulatorNode(BinaryReader Ar)
    {
        if (FModReader.Version >= 0x55)
        {
            Amount = Ar.ReadSingle();
            ThresholdMinimum = Ar.ReadSingle();
            ThresholdMaximum = Ar.ReadSingle();

            if (FModReader.Version >= 0x53)
            {
                AttackTime = Ar.ReadSingle();
                ReleaseTime = Ar.ReadSingle();

                if (FModReader.Version >= 0x7d)
                {
                    UseRMS = Ar.ReadBoolean();
                }
            }
        }
        else
        {
            Minimum = Ar.ReadSingle();
            Maximum = Ar.ReadSingle();
            ThresholdMinimum = Ar.ReadSingle();
            ThresholdMaximum = Ar.ReadSingle();
            EffectId = new FModGuid(Ar);
        }
    }
}