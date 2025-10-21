﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Objects;

public readonly struct FTimelineTempoMarker
{
    public readonly FModGuid BaseGuid;
    public readonly long TimeSignature;
    public readonly uint Position;
    public readonly float Tempo;

    public FTimelineTempoMarker(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        TimeSignature = Ar.ReadInt64();
        Position = Ar.ReadUInt32();
        Tempo = Ar.ReadSingle();
    }
}
