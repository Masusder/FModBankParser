namespace FModUEParser.Objects;

public readonly struct FModGuid
{
    public readonly uint Data1;
    public readonly ushort Data2;
    public readonly ushort Data3;
    public readonly byte[] Data4;

    public FModGuid(BinaryReader reader)
    {
        Data1 = reader.ReadUInt32();
        Data2 = reader.ReadUInt16();
        Data3 = reader.ReadUInt16();
        Data4 = reader.ReadBytes(8);
    }

    // For UE FGuid compatibility
    //public FModGuid(FGuid fguid)
    //{
    //    Data1 = fguid.A;
    //    Data2 = (ushort)((fguid.B >> 16) & 0xFFFF);
    //    Data3 = (ushort)(fguid.B & 0xFFFF);
    //    Data4 =
    //    [
    //            (byte)(fguid.C >> 24),
    //            (byte)(fguid.C >> 16),
    //            (byte)(fguid.C >> 8),
    //            (byte)(fguid.C),
    //            (byte)(fguid.D >> 24),
    //            (byte)(fguid.D >> 16),
    //            (byte)(fguid.D >> 8),
    //            (byte)(fguid.D)
    //    ];
    //}

    public bool Equals(FModGuid other)
    {
        return Data1 == other.Data1 &&
               Data2 == other.Data2 &&
               Data3 == other.Data3 &&
               Data4.AsSpan().SequenceEqual(other.Data4);
    }

    public override bool Equals(object? obj) => obj is FModGuid g && Equals(g);
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Data1);
        hash.Add(Data2);
        hash.Add(Data3);
        foreach (var b in Data4) hash.Add(b);
        return hash.ToHashCode();
    }

    public override readonly string ToString()
    {
        return $"{Data1:x8}-{Data2:x4}-{Data3:x4}-{BitConverter.ToString(Data4, 0, 2).Replace("-", "")}-{BitConverter.ToString(Data4, 2, 6).Replace("-", "")}";
    }

    public static bool operator ==(FModGuid left, FModGuid right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FModGuid left, FModGuid right)
    {
        return !(left == right);
    }
}
