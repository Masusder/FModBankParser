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
    public BusNode? BusBody;

    public BaseBusNode(BinaryReader Ar, bool includeRoutable)
    {
        BaseGuid = new FModGuid(Ar);
        if (includeRoutable) Routable = new FRoutable(Ar);
    }
}
