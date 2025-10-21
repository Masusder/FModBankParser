using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes;

public class CurveNode
{
    public readonly FModGuid BaseGuid;
    public readonly FModGuid OwnerGuid;
    public readonly FCurvePoint[] CurvePoints;

    public CurveNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        OwnerGuid = new FModGuid(Ar);
        CurvePoints = FModReader.ReadElemListImp<FCurvePoint>(Ar);
    }
}
