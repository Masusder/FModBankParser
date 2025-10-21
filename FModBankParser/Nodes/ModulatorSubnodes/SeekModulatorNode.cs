using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.ModulatorSubnodes;

public class SeekModulatorNode
{
    public readonly uint Flags;
    public readonly float SeekSpeedAscending;
    public readonly float SeekSpeedDescending;

    public SeekModulatorNode(BinaryReader Ar)
    {
        Flags = Ar.ReadUInt32();
        SeekSpeedAscending = Ar.ReadSingle();
        SeekSpeedDescending = Ar.ReadSingle();
    }
}
