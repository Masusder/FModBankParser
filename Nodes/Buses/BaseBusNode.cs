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
        var includeRoutable = false;
        BaseGuid = new FModGuid(Ar);

        if (includeRoutable)
        {
            // TODO
            return;
        }

        Ar.ReadUInt16(); // Payload size
        Routable = new FRoutable(Ar);
    }
}
