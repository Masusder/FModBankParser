using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class ParameterLayoutNode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid ParameterGuid;
    public readonly FModGuid LegacyGuid;
    public readonly FModGuid[] Instruments = [];
    public readonly uint Flags;
    public readonly FModGuid[] Controllers = [];
    public readonly FModGuid[] TriggerBoxes = [];

    public ParameterLayoutNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        ParameterGuid = new FModGuid(Ar);

        if (FModReader.Version < 0x6d) LegacyGuid = new FModGuid(Ar);
        if (FModReader.Version >= 0x82) Instruments = FModReader.ReadElemListImp<FModGuid>(Ar);

        Controllers = FModReader.ReadElemListImp<FModGuid>(Ar);

        if (FModReader.Version >= 0x6a) TriggerBoxes = FModReader.ReadElemListImp<FModGuid>(Ar);
    }
}
