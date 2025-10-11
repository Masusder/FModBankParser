using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.ModulatorSubnodes;

public class RandomModulatorNode
{
    public readonly float Amount;
    public readonly float Minimum;
    public readonly float Maximum;

    public RandomModulatorNode(BinaryReader Ar)
    {
        if (FModReader.Version >= 0x55)
        {
            Amount = Ar.ReadSingle();
        }
        else
        {
            Minimum = Ar.ReadSingle();
            Maximum = Ar.ReadSingle();
        }
    }
}
