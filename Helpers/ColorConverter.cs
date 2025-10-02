using Microsoft.Xna.Framework;

public static class ColorConverter
{
    public static Color HexToXnaColor(string hexString)
    {
        // Remove the '#' if it exists
        if (hexString.StartsWith("#"))
        {
            hexString = hexString.Substring(1);
        }

        // Ensure the string has 6 or 8 characters (RGB or ARGB)
        if (hexString.Length != 6 && hexString.Length != 8)
        {
            throw new System.ArgumentException("Invalid hex string format. Must be 6 or 8 characters (e.g., RRGGBB or AARRGGBB).");
        }

        byte a = 255; // Default to fully opaque
        byte r, g, b;

        if (hexString.Length == 8) // ARGB format
        {
            a = System.Convert.ToByte(hexString.Substring(0, 2), 16);
            r = System.Convert.ToByte(hexString.Substring(2, 2), 16);
            g = System.Convert.ToByte(hexString.Substring(4, 2), 16);
            b = System.Convert.ToByte(hexString.Substring(6, 2), 16);
        }
        else // RGB format
        {
            r = System.Convert.ToByte(hexString.Substring(0, 2), 16);
            g = System.Convert.ToByte(hexString.Substring(2, 2), 16);
            b = System.Convert.ToByte(hexString.Substring(4, 2), 16);
        }

        return new Color(r, g, b, a);
    }
}