

using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;

internal class Program
{
    static void Main(string[] args)
    {
        String file1 = "MSM Images\\monster_portrait_random.png";
        String file2 = "Capture2.png";
        Bitmap image1 = AForge.Imaging.Image.FromFile(file1);
        int screenWidth = 2560;
        int screenHeight = 1440;
        int imageSize = 265;
        
        Thread.Sleep(1000);
        Console.WriteLine("Started");

        System.Drawing.Image backgroundImage = CaptureScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
        System.Drawing.Image targetImage = CaptureScreen(100, 100, 0, 0, new Size(imageSize, imageSize));

        Bitmap backgroundBmp = new Bitmap(backgroundImage);
        Bitmap targetBmp = new Bitmap(file2);
        backgroundBmp = ConvertToFormat(backgroundBmp, PixelFormat.Format24bppRgb);
        targetBmp = ConvertToFormat(targetBmp, PixelFormat.Format24bppRgb);
        backgroundBmp.Save("output\\image" + 0 + ".png");

        CompareImages(backgroundBmp, targetBmp);
    }

    //This is a replacement for Cursor.Position in WinForms
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;

    //This simulates a left mouse click
    public static void LeftMouseClick(int xpos, int ypos)
    {
        SetCursorPos(xpos, ypos);
        mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
    }

    public static System.Drawing.Image CaptureScreen(int sourceX, int sourceY, int destX, int destY,
            Size regionSize)
    {
        Bitmap bmp = new Bitmap(regionSize.Width, regionSize.Height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(sourceX, sourceY, destX, destY, regionSize);
        }
        return bmp;
    }
    public static void CompareImages(Bitmap background, Bitmap target)
    {
        const Int32 divisor = 1;
        int num = 0;

        ExhaustiveTemplateMatching etm = new ExhaustiveTemplateMatching(0.95f);

        TemplateMatch[] tm = etm.ProcessImage(
            new ResizeNearestNeighbor(background.Width / divisor, background.Height / divisor).Apply(background),
            new ResizeNearestNeighbor(target.Width / divisor, target.Height / divisor).Apply(target)
            );

        BitmapData data = background.LockBits(
                new Rectangle(0, 0, background.Width, background.Height),
                ImageLockMode.ReadWrite, background.PixelFormat);
        foreach (TemplateMatch m in tm)
        {
            Drawing.Rectangle(data, m.Rectangle, Color.White);

            Console.WriteLine(m.Rectangle.Location.ToString());
            System.Drawing.Image outputImg = CaptureScreen(m.Rectangle.X, m.Rectangle.Y, 0, 0, new Size(m.Rectangle.Width, m.Rectangle.Height));
            Bitmap outputMap = new Bitmap(outputImg);
            outputMap.Save("output\\image" + num + ".png");
            num++;
        }
        background.UnlockBits(data);
    }
    public static Bitmap ConvertToFormat(System.Drawing.Image image, PixelFormat format)
    {
        Bitmap copy = new Bitmap(image.Width, image.Height, format);
        using (Graphics gr = Graphics.FromImage(copy))
        {
            gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
        }
        return copy;
    }
}
