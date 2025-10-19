using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class MappingNode
{
    public readonly FModGuid BaseGuid;
    public readonly FMappingPoint[] MappingPoints;

    public MappingNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        MappingPoints = FModReader.ReadElemListImp<FMappingPoint>(Ar);
    }
}
