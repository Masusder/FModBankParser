using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Enums;

public enum EStringTableType : uint
{
    StringTable_RadixTree_32Bit = 0x0,
    StringTable_RadixTree_24Bit = 0x1,
    StringTable_Max = 0x2
}
