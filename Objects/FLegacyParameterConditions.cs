using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public class FLegacyParameterConditions
{
    public readonly FModGuid BaseGuid;
    public readonly float Minimum;
    public readonly float Maximum;

    public FLegacyParameterConditions(BinaryReader Ar)
    {
        BaseGuid = new FModGuid();
        Minimum = Ar.ReadSingle();
        Maximum = Ar.ReadSingle();
    }
}
