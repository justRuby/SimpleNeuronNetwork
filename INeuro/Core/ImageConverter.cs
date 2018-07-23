using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeuro.Core
{
    public static class ImageConverter
    {
        public static bool isMultipleImg;
        
        public static int[,] ToByte(this Image img)
        {
            bool IsWhite = false;
            var bmp = new Bitmap(img);
            int[,] array = new int[bmp.Width, bmp.Height];

            for (int y = 0; y < bmp.Width; y++)
            {
                for (int x = 0; x < bmp.Height; x++)
                {
                    IsWhite = bmp.GetPixel(y, x).R >= 230 && bmp.GetPixel(y, x).G >= 230 && bmp.GetPixel(y, x).B >= 230;
                    array[x, y] = IsWhite ? 0 : 1;
                }
            }


            //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);

            //unsafe
            //{
            //    byte* ptr = (byte*)bmpData.Scan0;

            //    for (int y = 0; y < bmp.Height; y++)
            //    {

            //        var row = ptr + (y * bmpData.Stride);

            //        for (int x = 0; x < bmp.Width; x++)
            //        {
            //            var pixel = row + y * 4;

            //            IsWhite = pixel[0] >= 230 && pixel[1] >= 230 && pixel[2] >= 230;
            //            array[x, y] = IsWhite ? 0 : 1;
            //        }
            //    }
            //}

            return array;
        }
        public static Image ToImage(this int[,] img)
        {
            Bitmap bmp = new Bitmap(img.GetLength(0), img.GetLength(1));

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    bmp.SetPixel(x, y, img[x, y] == 0 ? Color.White : Color.Black);
                }
            }

            return bmp;
        }
        public static int[,] CutImage(this int[,] bytes)
        {
            Rectangle r = GetRect(bytes);

            int[,] array = new int[r.Width, r.Height];

            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    array[x, y] = bytes[x + r.X, y + r.Y];
                }
            }

            return array;
        }
        public static Image ScaleImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public static int[,] ToInput(this Image source, int width, int height)
        {
            return source.ToByte().CutImage().ToImage().ScaleImage(width, height).ToByte();
        }

        private static Rectangle GetRect(int[,] bytes)
        {
            return new Rectangle(GetRectX(bytes), GetRectY(bytes), GetRectX2(bytes), GetRectY2(bytes));
        }

        private static int GetRectX(int[,] bytes)
        {
            int position = 0;

            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                for (int j = 0; j < bytes.GetLength(1); j++)
                {
                    if (bytes[i, j] == 1)
                    {
                        position = i;
                        return position;
                    }
                }
            }

            return 0;
        }
        private static int GetRectX2(int[,] bytes)
        {
            int position = 0;

            for (int i = bytes.GetLength(0) - 1; i > 0; i--)
            {
                for (int j = bytes.GetLength(1) - 1; j > 0; j--)
                {
                    if (bytes[i, j] == 1)
                    {
                        position = i;
                        return position;
                    }
                }
            }

            return 0;
        }
        private static int GetRectY(int[,] bytes)
        {
            int position = 0;

            for (int i = 0; i < bytes.GetLength(1); i++)
            {
                for (int j = 0; j < bytes.GetLength(0); j++)
                {
                    if (bytes[i, j] == 1)
                    {
                        position = i;
                        return position;
                    }
                }
            }

            return 0;
        }
        private static int GetRectY2(int[,] bytes)
        {
            int position = 0;

            for (int i = bytes.GetLength(1) - 1; i > 0; i--)
            {
                for (int j = bytes.GetLength(0) - 1; j > 0; j--)
                {
                    if (bytes[i, j] == 1)
                    {
                        position = i;
                        return position;
                    }
                }
            }

            return 0;
        }
    }
}
