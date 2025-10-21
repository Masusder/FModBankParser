using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FLegacyParameterConditions
{
    public readonly FModGuid BaseGuid;
    public readonly float Minimum;
    public readonly float Maximum;

    public FLegacyParameterConditions(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Minimum = Ar.ReadSingle();
        Maximum = Ar.ReadSingle();
    }
}
