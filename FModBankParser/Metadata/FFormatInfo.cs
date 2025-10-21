using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FModBankParser.Metadata;

public readonly struct FFormatInfo
{
    public readonly int FileVersion;
    public readonly int CompatVersion;

    public FFormatInfo(BinaryReader Ar)
    {
        FileVersion = Ar.ReadInt32();
        Debug.WriteLine($"Soundbank version: 0x{FileVersion:X}");
        CompatVersion = Ar.ReadInt32();
    }
}
