using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class HashDataNode
{
    public readonly FHashData[] HashData = [];

    public HashDataNode(BinaryReader Ar)
    {
        HashData = FModReader.ReadElemListImp<FHashData>(Ar);
    }
}
