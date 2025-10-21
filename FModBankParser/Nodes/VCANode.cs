using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes;

public class VCANode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid[] Strips;
    public readonly FMixerStrip MixerStrip;

    public VCANode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Strips = FModReader.ReadElemListImp<FModGuid>(Ar);
        MixerStrip = new FMixerStrip(Ar);
    }
}
