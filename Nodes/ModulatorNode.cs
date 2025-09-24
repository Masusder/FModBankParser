using FModUEParser.Nodes.ModulatorSubnodes;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class ModulatorNode
{
    public readonly EModulatorType Type;
    public readonly int PropertyIndex;
    public readonly EPropertyType PropertyType;
    public readonly EClockSource ClockSource;
    public object Subnode{ get; }

    public ModulatorNode(BinaryReader Ar)
    {
        Type = (EModulatorType)Ar.ReadInt32();
        PropertyIndex = Ar.ReadInt32();
        PropertyType = (EPropertyType)Ar.ReadInt32();
        ClockSource = (EClockSource)Ar.ReadInt32();

        if (Type > EModulatorType.SpectralSidechain)
        {
            //goto hell;
        }

        Subnode = Type switch
        {
            EModulatorType.ADSR => new ADSRModulatorNode(Ar),
            //EModulatorType.Random => new RandomModulatorNode(),
            EModulatorType.Envelope => new EnvelopeModulatorNode(Ar),
            //EModulatorType.LFO => new LFOModulatorNode(),
            //EModulatorType.Seek => new SeekModulatorNode(),
            EModulatorType.SpectralSidechain => new SpectralSidechainModulatorNode(Ar),
            _ => throw new InvalidDataException($"Unknown Modulator type {Type}")
        };
    }
}
