using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public enum EClockSource : uint
{
    ClockSource_Local = 0x0,
    ClockSource_Global = 0x1,
    ClockSource_Max = 0x2
}
