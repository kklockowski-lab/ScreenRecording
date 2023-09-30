using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

public static class Marker
{
    private static Graphics graphics;
    private static Graphics gCropped;
    private static Bitmap croppedImage;
    private static Rectangle sourceRect;

    private static Thread thread;

    private static int rad;
    private static int X;
    private static int Y;
    private static int buff = 3;

    private static object syncObj = new object();
    public static void DrawMarker(Point location, int radius)
    {
        lock (syncObj)
        {
            X = location.X;
            Y = location.Y;

            rad = radius;
            graphics = Graphics.FromHwnd(IntPtr.Zero); // Uzyskaj uchwyt do pulpitu
            graphics.SmoothingMode = SmoothingMode.AntiAlias; // Ustaw tryb wygładzania

            //Kopia obrazu - ramka
            sourceRect = new Rectangle(location.X - radius, location.Y - radius, 2 * radius, 2 * radius);
            croppedImage = new Bitmap(sourceRect.Width, sourceRect.Height);
            // Utwórz Graphics dla wyciętego obrazu

            gCropped = Graphics.FromImage(croppedImage);
            // Skopiuj fragment ekranu do wyciętego obrazu

            gCropped.CopyFromScreen(sourceRect.Location, Point.Empty, sourceRect.Size);


            //// Narysuj kółko
            //Brush brush = new SolidBrush(Color.Yellow);
            //graphics.FillEllipse(brush, location.X - radius, location.Y - radius, 2 * radius, 2 * radius);

            thread = new Thread(new ThreadStart(Reset));
            thread.Start();
        }
    }

    private static void Reset()
    {
        lock (syncObj)
        {
            Rectangle destinationRect = new Rectangle(sourceRect.X, sourceRect.Y, croppedImage.Width, croppedImage.Height);
            Brush brush = new SolidBrush(Color.Yellow);
            //  graphics.DrawImage(croppedImage, destinationRect);


            for (int i = rad; i > 0; i = i - 2)
            {
                //  Graphics clonedGraphics = Graphics.FromHwnd(gCropped.GetHdc());
                //  graphics.DrawImage(croppedImage, destinationRect);

                //  graphics.FillEllipse(brush, X - i, Y - i, 2 * i, 2 * i);
                //  clonedGraphics.FillEllipse(brush, 0, 0, 2 * i, 2 * i);

                using (Bitmap bitmap = new Bitmap(croppedImage.Width, croppedImage.Height))
                {
                    using (Graphics tempGraphics = Graphics.FromImage(bitmap))
                    {
                        tempGraphics.DrawImage(croppedImage, 0, 0, croppedImage.Width, croppedImage.Height);
                        // tempGraphics.Clear(Color.White);
                        tempGraphics.FillEllipse(brush, rad - i, rad - i, 2 * i, 2 * i);
                    }

                    Image clonedImage = (Image)bitmap.Clone();

                    graphics.DrawImage(clonedImage, destinationRect);
                    // Teraz masz obiekt Image w zmiennej 'clonedImage'
                }



                //  Thread.Sleep(500);

                // Narysuj wycięty obraz na oryginalnym Graphics
                //graphics.DrawImage(croppedImage, destinationRect);
                // break;
            }
            graphics.DrawImage(croppedImage, destinationRect);
        }
    }

}
