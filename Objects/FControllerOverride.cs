namespace FModUEParser.Objects;

public readonly struct FControllerOverride
{
    public readonly FModGuid ControllerGuid;
    public readonly FModGuid CurveGuid;

    public FControllerOverride(BinaryReader Ar)
    {
        ControllerGuid = new FModGuid(Ar);
        CurveGuid = new FModGuid(Ar);
    }
}