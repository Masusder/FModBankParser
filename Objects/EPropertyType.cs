using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public enum EPropertyType : uint
{
    PropertyType_Normal = 0x0,
    PropertyType_Volume = 0x1,
    PropertyType_Undefined = 0x2,
    PropertyType_Max = 0x3
}
