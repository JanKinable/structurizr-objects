﻿namespace StructurizrObjects.Utils
{
    public static class ColorExtension
    {
        public static string ToHex(this System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        
    }
}