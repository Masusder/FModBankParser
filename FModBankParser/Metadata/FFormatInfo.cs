using FModBankParser.Enums;
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
        var latestVersion = (int)EFModVersion.NEWEST_SUPPORTED_FILEVERSION;
        if (FileVersion > latestVersion)
        {
            Debug.WriteLine($"FMod version 0x{FileVersion:X} is not supported, latest supported version is 0x{latestVersion:X}");
        }
        CompatVersion = Ar.ReadInt32();
    }
}
