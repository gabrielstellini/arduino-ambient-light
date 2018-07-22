using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbientLight
{
    internal static class Program
    {
        private static int x;
        private static int y;

        private static int avgR = 255;
        private static int avgG = 255;
        private static int avgB = 255;
        private static SerialPort port;


        static void Main(string[] args)
        {
            x = Screen.PrimaryScreen.Bounds.Width;
            y = Screen.PrimaryScreen.Bounds.Height;

            var bmp = new Bitmap(x, y);
            var gr = Graphics.FromImage(bmp);
            port = new SerialPort("COM4", 2000000);
            port.Open();

            while (true)
            {
                try
                {
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
//                  bmp.Save("img.png", System.Drawing.Imaging.ImageFormat.Png);
                    GetAverageColour2(bmp);
                    sendColours();
                    Thread.Sleep(20);
                }
                catch (Exception e)
                {
                    //Exception may occur on lock screen or "run as admin" screen - in that case, just retry
                    Console.Write("Exception caught - retrying");
                    Console.Write(e);
                }
            }
        }


        public static void GetAverageColour(Bitmap bmp)
        {
            var skipValue = 1;

            float r = 0;
            float g = 0;
            float b = 0;

            for (var i = 0; i < x; i = i + skipValue)
            {
                for (var j = 0; j < y; j = j + skipValue)
                {
                    var pixel = bmp.GetPixel(i, j); //the ARGB integer has the colors of pixel (i,j)
                    r = r + pixel.R;
                    g = g + pixel.G;
                    b = b + pixel.B;
                }
            }


            var aX = x / skipValue;
            var aY = y / skipValue;

            r = r / (aX * aY); //average red 
            g = g / (aX * aY); //average green
            b = b / (aX * aY); //average blue

            // filter values to increase saturation
            float maxColorInt;
            float minColorInt;

            maxColorInt = new[] { r, g, b }.Max();
            if (maxColorInt == r)
            {
                // red
                if (maxColorInt < 225 - 20)
                    r = maxColorInt + 20;
            }
            else if (maxColorInt == g)
            {
                //green
                if (maxColorInt < 225 - 20)
                    g = maxColorInt + 20;
            }
            else
            {
                //blue
                if (maxColorInt < 225 - 20)
                    b = maxColorInt + 20;
            }


            //minimise smallest
            minColorInt = new[] { r, g, b }.Min();
            if (minColorInt == r)
            {
                // red
                if (minColorInt > 20)
                    r = minColorInt - 20;
            }
            else if (minColorInt == g)
            {
                //green
                if (minColorInt > 20)
                    g = minColorInt - 20;
            }
            else
            {
                //blue
                if (minColorInt > 20)
                    b = minColorInt - 20;
            }


            avgR = (short)r;
            avgG = (short)g;
            avgB = (short)b;
        }

        static unsafe void GetAverageColour2(Bitmap bm)
        {
            BitmapData srcData = bm.LockBits(
                new Rectangle(0, 0, bm.Width, bm.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = bm.Width;
            int height = bm.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }

            float b = totals[0] / (width * height);
            float g = totals[1] / (width * height);
            float r = totals[2] / (width * height);

            // filter values to increase saturation
            float maxColorInt;
            float minColorInt;

            maxColorInt = new[] { r, g, b }.Max();
            if (maxColorInt == r)
            {
                // red
                if (maxColorInt < 225 - 20)
                    r = maxColorInt + 20;
            }
            else if (maxColorInt == g)
            {
                //green
                if (maxColorInt < 225 - 20)
                    g = maxColorInt + 20;
            }
            else
            {
                //blue
                if (maxColorInt < 225 - 20)
                    b = maxColorInt + 20;
            }


            //minimise smallest
            minColorInt = new[] { r, g, b }.Min();
            if (minColorInt == r)
            {
                // red
                if (minColorInt > 20)
                    r = minColorInt - 20;
            }
            else if (minColorInt == g)
            {
                //green
                if (minColorInt > 20)
                    g = minColorInt - 20;
            }
            else
            {
                //blue
                if (minColorInt > 20)
                    b = minColorInt - 20;
            }


            avgR = (short)r;
            avgG = (short)g;
            avgB = (short)b;

            Console.Write(" || " + avgR +" "+avgG +" "+avgB);

            bm.UnlockBits(new BitmapData());
        }


        private static void sendColours()
        {
            //Keep trying to connect
            bool retry = true;
            while (retry)
            {
                try
                {
                    port.Write("<" + avgR + "," + avgG + "," + avgB + ">");
//                    Console.Write("<" + avgR + "," + avgG + "," + avgB + ">");
                    retry = false;
                }
                catch
                {
                    port.Close();
                    port = new SerialPort("COM4", 2000000);
                    port.Open();
                }
            }
        }

//        private static void sendRandomColours()
//        {
//
//            for (int r = 0; r < 255; r++)
//            {
//                for (int g = 0; g < 255; g++)
//                {
//                    for (int b = 0; b < 255; b++)
//                    {
//                        bool retry = true;
//                        while (retry)
//                        {
//                            try
//                            {
//                                port.Write("<" + r + "," + g + "," + b + ">");
//                                retry = false;
//                                Thread.Sleep(2);
//                            }
//                            catch
//                            {
//                                port.Close();
//                                port = new SerialPort("COM4", 2000000);
//                                port.Open();
//                            }
//                        }
//                    }
//                }
//            }
//
//            
//        }


    }
}
