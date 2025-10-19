using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class ControllerOwnerNode
{
    public readonly FModGuid[] Controllers;

    public ControllerOwnerNode(BinaryReader Ar)
    {
        Controllers = FModReader.ReadElemListImp<FModGuid>(Ar);
    }
}
