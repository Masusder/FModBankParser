using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FEventParameterStub
{
    public readonly uint StubIndex;
    public readonly FModGuid ParameterGuid;
    public readonly float InitialValue;

    public FEventParameterStub(BinaryReader Ar)
    {
        StubIndex = Ar.ReadUInt32();
        ParameterGuid = new FModGuid(Ar);
        InitialValue = Ar.ReadSingle();
    }
}