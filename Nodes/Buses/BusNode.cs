using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Buses;

public class BusNode
{
    public readonly uint Flags;
    public readonly uint InputChannelLayout;
    public readonly FModGuid[] PreFaderEffects;
    public readonly FModGuid[] PostFaderEffects;
    public readonly FMixerStrip MixerStrip;

    public readonly int MaximumPolyphony;
    public readonly int PolyphonyLimitBehavior;
    public readonly uint[] PreFaderInputChannelLayouts;
    public readonly uint[] PostFaderInputChannelLayouts;

    public readonly int ObjectPannerIndex;
    public readonly EPortType PortType;

    public BusNode(BinaryReader Ar)
    {
        if (FModReader.Version >= 0x8c)
        {
            Flags = Ar.ReadUInt32();
        }
        else
        {
            Flags = Ar.ReadByte();
        }

        InputChannelLayout = Ar.ReadUInt32();
        PreFaderEffects = FModReader.ReadElemListImp<FModGuid>(Ar);
        PostFaderEffects = FModReader.ReadElemListImp<FModGuid>(Ar);
        MixerStrip = new FMixerStrip(Ar);

        MaximumPolyphony = Ar.ReadInt32();
        PolyphonyLimitBehavior = Ar.ReadInt32();

        PreFaderInputChannelLayouts = FModReader.ReadFixElemList(Ar, Ar => Ar.ReadUInt32());
        PostFaderInputChannelLayouts = FModReader.ReadFixElemList(Ar, Ar => Ar.ReadUInt32());

        if (FModReader.Version >= 0x6f)
        {
            ObjectPannerIndex = Ar.ReadInt32();
        }

        if (FModReader.Version >= 0x8c)
        {
            PortType = (EPortType)Ar.ReadUInt32();
        }
    }
}
