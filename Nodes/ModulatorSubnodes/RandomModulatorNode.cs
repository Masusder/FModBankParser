using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.ModulatorSubnodes;

public class RandomModulatorNode
{
    public readonly float Minimum;
    public readonly float Maximum;
    public readonly float Amount;

    public RandomModulatorNode(BinaryReader Ar)
    {
        Minimum = Ar.ReadSingle();
        Maximum = Ar.ReadSingle();
        Amount = Ar.ReadSingle();
    }
}
