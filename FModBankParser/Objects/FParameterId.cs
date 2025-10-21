using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FParameterId
{
    public readonly uint Data1;
    public readonly uint Data2;

    public FParameterId(BinaryReader Ar)
    {
        Data1 = Ar.ReadUInt32();
        Data2 = Ar.ReadUInt32();
    }
}