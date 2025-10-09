using FModUEParser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FParentContext
{
    public readonly ENodeId NodeId;
    public readonly FModGuid Guid;

    public FParentContext(ENodeId nodeId, FModGuid guid)
    {
        NodeId = nodeId;
        Guid = guid;
    }
}
