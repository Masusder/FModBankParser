using FModUEParser.Enums;

namespace FModUEParser.Objects;

public readonly struct FEvaluator
{
    public readonly EEvaluatorType Type;
    public readonly object? Data;

    public FEvaluator(BinaryReader Ar)
    {
        uint rawType = Ar.ReadUInt32();
        Type = (EEvaluatorType)(rawType & 0xFF); // Only lower 8 bits are used for the type
        Data = null;

        switch (Type)
        {
            case EEvaluatorType.Basic0:
            case EEvaluatorType.Basic1:
            case EEvaluatorType.Basic2:
            case EEvaluatorType.Basic3:
            case EEvaluatorType.Type12:
            case EEvaluatorType.Type30:
                break;

            case EEvaluatorType.Type10:
                Data = Ar.ReadUInt32();
                break;

            case EEvaluatorType.Type11:
                Data = new FModGuid(Ar);
                break;

            case EEvaluatorType.Type20:
                Data = new uint[]
                {
                        Ar.ReadUInt32(),
                        Ar.ReadUInt32()
                }; ;
                break;

            default:
                Console.WriteLine($"Unknown evaluator type: {Type} (Raw: {rawType})");
                break;
        }
    }
}