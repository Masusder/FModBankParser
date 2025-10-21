using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FMixerStrip
{
    public readonly float Volume;
    public readonly float Pitch;
    public readonly FModGuid[] VCAs = [];

    public FMixerStrip(BinaryReader Ar)
    {
        Ar.ReadUInt16(); // Payload size
        Volume = Ar.ReadSingle();
        Pitch = Ar.ReadSingle();

        if (FModReader.Version >= 0x6c)
        {
            VCAs = FModReader.ReadElemListImp<FModGuid>(Ar);
        }
    }
}
