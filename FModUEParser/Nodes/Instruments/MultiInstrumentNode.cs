using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Instruments;

public class MultiInstrumentNode : BaseInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public PlaylistNode? PlaylistBody;

    public MultiInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
    }
}
