using FModUEParser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public class FParentContext
{
    public ENodeId NodeId { get; }
    public FModGuid Guid { get; }

    public FParentContext(ENodeId nodeId, FModGuid guid)
    {
        NodeId = nodeId;
        Guid = guid;
    }
}
