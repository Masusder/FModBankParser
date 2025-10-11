using FModUEParser.Enums;
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
    public readonly FModGuid BaseGuid;
    public readonly FModGuid OwnerGuid;
    public readonly int PropertyIndex;
    public readonly EModulatorType Type;
    public readonly EPropertyType PropertyType;
    public readonly EClockSource ClockSource;
    public readonly object? Subnode;

    public ModulatorNode(BinaryReader Ar)
    {
        Ar.ReadUInt16(); // Payload size
        BaseGuid = new FModGuid(Ar);
        OwnerGuid = new FModGuid(Ar);

        PropertyIndex = Ar.ReadInt32();
        Type = (EModulatorType)Ar.ReadInt32();

        if (FModReader.Version < 0x55)
        {
            PropertyType = (EPropertyType)Ar.ReadInt32();
            ClockSource = (EClockSource)Ar.ReadInt32();
        }
        else
        {
            PropertyType = (EPropertyType)Ar.ReadInt32();
            ClockSource = EClockSource.ClockSource_Local;
        }

        // TODO: something is still off
        switch (Type)
        {
            case EModulatorType.ADSR:
                Subnode = new ADSRModulatorNode(Ar);
                break;
            case EModulatorType.Random:
                Subnode = new RandomModulatorNode(Ar);
                break;
            case EModulatorType.Envelope:
                Subnode = new EnvelopeModulatorNode(Ar);
                break;
            case EModulatorType.LFO:
                Subnode = new LFOModulatorNode(Ar);
                break;
            case EModulatorType.Seek:
                Subnode = new SeekModulatorNode(Ar);
                break;
            case EModulatorType.SpectralSidechain:
                Subnode = new SpectralSidechainModulatorNode(Ar);
                break;
            default:
                Console.WriteLine($"Unhandled modulator type {Type} ({(int)Type}) at stream position {Ar.BaseStream.Position}");
                break;
        }
    }
}
