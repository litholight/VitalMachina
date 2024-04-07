namespace Mario.Common.Models
{
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        // Common Colors
        public static Color Red => new Color(255, 0, 0);
        public static Color Green => new Color(0, 255, 0);
        public static Color Blue => new Color(0, 0, 255);
        public static Color Orange => new Color(255, 165, 0);
        public static Color Brown => new Color(139, 69, 19);
        public static Color White => new Color(255, 255, 255);

        // Add other common colors as needed

        // Method to convert Color to CSS-compatible hex string
        public string ToHexString()
        {
            return $"#{R:X2}{G:X2}{B:X2}{A:X2}";
        }
    }
}
