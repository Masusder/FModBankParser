namespace FModUEParser.Objects
{
    public struct FTriggerBox
    {
        public FModGuid Guid;
        public uint E;       // next 4 bytes (offset 0x10)
        public uint F;       // next 4 bytes (offset 0x14)

        public FTriggerBox(BinaryReader Ar)
        {
            Guid = new FModGuid(Ar);
            E = Ar.ReadUInt32();
            F = Ar.ReadUInt32();
        }
    }
}