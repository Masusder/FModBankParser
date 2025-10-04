using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class WaveformResourceNode
{
    public readonly FModGuid BaseGuid;
    public readonly int SubsoundIndex;
    public readonly int SoundBankIndex;
    public readonly EWaveformLoadingMode LoadingMode;

    public WaveformResourceNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Ar.ReadUInt16(); // It's payload size after guid
        SubsoundIndex = Ar.ReadInt32();
        SoundBankIndex = Ar.ReadInt32();
        if (FModReader.Version >= 0x46)
        {
            LoadingMode = (EWaveformLoadingMode)Ar.ReadUInt32();
        }
    }
}
