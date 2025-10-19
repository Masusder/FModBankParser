using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Instruments;

public class EffectInstrumentNode : BaseInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid EffectGuid;

    public EffectInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        EffectGuid = new FModGuid(Ar);
    }
}
