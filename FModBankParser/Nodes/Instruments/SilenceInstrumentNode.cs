using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Instruments;

public class SilenceInstrumentNode : BaseInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly float Duration;

    public SilenceInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Duration = Ar.ReadSingle();
    }
}
