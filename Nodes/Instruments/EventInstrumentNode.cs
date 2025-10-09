using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Instruments;

public class EventInstrumentNode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid EventGuid;
    public readonly float SnapshotIntensity;
    public readonly FEventParameterStub[] EventParameterStubs;

    public EventInstrumentNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        EventGuid = new FModGuid(Ar);
        SnapshotIntensity = Ar.ReadSingle();
        EventParameterStubs = FModReader.ReadElemListImp<FEventParameterStub>(Ar);
    }
}
