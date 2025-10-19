﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class EffectNode
{
    public readonly uint Flags;
    public readonly float WetMix;
    public readonly float WetLevel;
    public readonly float DryLevel;
    public readonly float InputGain;

    public EffectNode(BinaryReader Ar)
    {
        Flags = Ar.ReadUInt32();
        WetMix = Ar.ReadSingle();
        WetLevel = Ar.ReadSingle();
        DryLevel = Ar.ReadSingle();
        InputGain = Ar.ReadSingle();
    }
}
