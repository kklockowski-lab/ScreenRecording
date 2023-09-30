using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public class Marker
{
    public static void DrawMarker(Point location, int radius)
    {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) // Uzyskaj uchwyt do pulpitu
        {
            g.SmoothingMode = SmoothingMode.AntiAlias; // Ustaw tryb wygładzania
            using (Pen pen = new Pen(Color.Yellow, 3))
            {
                g.DrawEllipse(pen, location.X - radius, location.Y - radius, 2 * radius, 2 * radius); // Narysuj kółko
            }
        }
    }
}
