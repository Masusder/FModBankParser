using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Buses;

public class BaseBusNode
{
    public readonly FModGuid BaseGuid;
    public readonly FRoutable Routable;

    public BaseBusNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Ar.ReadUInt16(); // Payload size
        Routable = new FRoutable(Ar);
    }
}
