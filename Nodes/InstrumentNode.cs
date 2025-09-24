using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class InstrumentNode
{
    public FModGuid BaseGuid { get; }

    public InstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
    }
}
