using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Instruments;

public class WaveformInstrumentNode : BaseInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly EWaveformLoadingMode LegacyLoadingMode;
    public readonly FModGuid WaveformResourceGuid;

    public WaveformInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        if (FModReader.Version < 0x46)
        {
            LegacyLoadingMode = (EWaveformLoadingMode)Ar.ReadUInt32();
        }
        WaveformResourceGuid = new FModGuid(Ar);
    }
}
