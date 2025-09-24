using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public enum EModulatorType : int
{
    ADSR = 0,
    Random = 1,
    Envelope = 2,
    LFO = 3,
    Seek = 4,
    SpectralSidechain = 5
}
