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

    private static int _radius;
    private static object syncObj = new object();
    public static void DrawMarker(Point location, int radius, IntPtr? intPoint = null)
    {
        lock (syncObj)
        {
            _radius = radius;
            if (intPoint == null)
                graphics = Graphics.FromHwnd(IntPtr.Zero); // Uzyskaj uchwyt do pulpitu
            else 
                graphics = Graphics.FromHwnd((IntPtr)intPoint);

            //Kopia obrazu - ramka
            sourceRect = new Rectangle(location.X - radius, location.Y - radius, 2 * radius, 2 * radius);
            croppedImage = new Bitmap(sourceRect.Width, sourceRect.Height);


            gCropped = Graphics.FromImage(croppedImage);

            // Skopiuj fragment ekranu do wyciętego obrazu
            gCropped.CopyFromScreen(sourceRect.Location, Point.Empty, sourceRect.Size);

            // thread = new Thread(new ThreadStart(Reset));
            ThreadPool.QueueUserWorkItem(new WaitCallback(Reset), null);
            // thread.Start();
        }
    }

    private static void Reset(object state)
    {
        lock (syncObj)
        {
            Rectangle destinationRect = new Rectangle(sourceRect.X, sourceRect.Y, croppedImage.Width, croppedImage.Height);
            Brush brush = new SolidBrush(Color.Yellow);

            for (int i = _radius / 2; i > 0; i--)
            {
                using (Bitmap bitmap = new Bitmap(croppedImage.Width, croppedImage.Height))
                {
                    using (Graphics tempGraphics = Graphics.FromImage(bitmap))
                    {
                        tempGraphics.DrawImage(croppedImage, 0, 0, croppedImage.Width, croppedImage.Height);
                        tempGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                        tempGraphics.FillEllipse(brush, _radius - i, _radius - i, 2 * i, 2 * i);
                    }

                    Image clonedImage = (Image)bitmap.Clone();
                    graphics.DrawImage(clonedImage, destinationRect);

                    // Thread.Sleep(40);
                }
            }
            graphics.DrawImage(croppedImage, destinationRect);
        }
    }

}
