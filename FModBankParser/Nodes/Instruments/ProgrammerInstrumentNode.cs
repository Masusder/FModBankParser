using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Instruments;

public class ProgrammerInstrumentNode : BaseInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly string Name = string.Empty;

    public ProgrammerInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Name = FModReader.ReadString(Ar);
    }
}
