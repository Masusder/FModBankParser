using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Instruments;

public class CommandInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly uint CommandType;
    public readonly FModGuid TargetGuid;
    public readonly float Value;

    public CommandInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        CommandType = Ar.ReadUInt32();
        TargetGuid = new FModGuid(Ar);

        if (FModReader.Version >= 0x80)
        {
            Value = Ar.ReadSingle();
        }
    }
}
