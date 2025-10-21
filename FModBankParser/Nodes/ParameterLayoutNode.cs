using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes;

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

        if (FModReader.Version >= 0x71) Controllers = FModReader.ReadElemListImp<FModGuid>(Ar);

        if (FModReader.Version >= 0x6a)
        {
            TriggerBoxes = FModReader.ReadElemListImp<FModGuid>(Ar);
        }
        else
        {
            // Convert legacy trigger boxes to new format
            var legacy = FModReader.ReadElemListImp<FLegacyTriggerBox>(Ar);
            var converted = new FModGuid[legacy.Length];
            for (int i = 0; i < legacy.Length; i++)
            {
                converted[i] = legacy[i].InstrumentGuid;
            }
            TriggerBoxes = converted;
        }
    }
}
