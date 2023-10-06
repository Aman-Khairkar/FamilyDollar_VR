namespace JDA
{
    /// <summary>
    /// Colors in JDA are represented as 24-bit integers.  The PfaColor class converts the JDA colors to RGB format.
    /// </summary>
    public class PfaColor
    {
        public int R;
        public int G;
        public int B;

        public PfaColor(int r, int g, int b) {
            R = r;
            G = g;
            B = b;
        }

        public PfaColor(long c) {
            R = (int)(c & 0x000000FF);
            G = (int)((c & 0x0000FF00) >> 8);
            B = (int)((c & 0x00FF0000) >> 16);
        }

        public override string ToString()
        {
            long result = 0;

            result |= (uint)R;
            result |= (uint)G << 8;
            result |= (uint)B << 16;

            return result.ToString();
        }

        public static PfaColor FromRGB(int r, int g, int b) {
            return new PfaColor(r, g, b);
        }
    }
}
