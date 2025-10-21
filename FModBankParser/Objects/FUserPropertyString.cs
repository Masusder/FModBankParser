using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FUserPropertyString
{
    public readonly string Key;
    public readonly string Value;

    public FUserPropertyString(BinaryReader Ar)
    {
        Key = FModReader.ReadString(Ar);
        Value = FModReader.ReadString(Ar);
    }
}
