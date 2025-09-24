namespace FModUEParser.Objects;

public struct FQuantization
{
    public readonly EQuantizationUnit Unit { get; }
    public readonly int Multiplier { get; }

    public FQuantization(BinaryReader Ar)
    {
        Unit = (EQuantizationUnit)Ar.ReadUInt32();

        if (Unit <= EQuantizationUnit.EighthNote)
        {
            Multiplier = Ar.ReadInt32();
        }
        else
        {
            Multiplier = 0;
        }
    }
}
