namespace Steam3Kit.Types
{
    class BitVector64
    {
        public ulong Data { get; set; }

        public BitVector64(ulong value)
        {
            Data = value;
        }

        public ulong this[uint bitoffset, ulong valuemask]
        {
            get => (Data >> (ushort)bitoffset) & valuemask;
            set => Data = (Data & ~(valuemask << (ushort)bitoffset)) | ((value & valuemask) << (ushort)bitoffset);
        }
    }
}
