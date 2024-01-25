using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pirmas_laboratorinis
{

    //internal class Program
    //{
    //    static void Main(string[] args)
    //    {
    //    }
    //}
    internal class Renderer
    {
        public Renderer(string OutputName, ushort Width, ushort Height, uint FillingColor) // Color format is ARGB (to define recomended hex: 0xAARRGGBB), coordinates start from bottom left corner, 1 unit is 1 pixel
        {
            this.Width = Width;
            this.Height = Height;
            Buffer = new uint[Width * Height];

            Array.Fill(Buffer, FillingColor);

            this.OutputName = OutputName;
            if (!OutputName.Contains(".bmp"))
                this.OutputName += ".bmp";
        }
        public void DrawFilledTriangle(double X0, double Y0, double X1, double Y1, double X2, double Y2, double Precision = 0.5, uint Color = 0)
        {
            double Length = Math.Sqrt(Math.Pow(X1 - X0, 2) + Math.Pow(Y1 - Y0, 2));

            double XStep = (X1 - X0) / (Length / Precision);
            double YStep = (Y1 - Y0) / (Length / Precision);

            double XRun = X0;
            double YRun = Y0;
            for (double i = 0; i < Length; i += Precision)
            {
                XRun += XStep;
                YRun += YStep;

                DrawLine(XRun, YRun, X2, Y2, Precision, Color);
            }
        }
        public void DrawLine(double X0, double Y0, double X1, double Y1, double Precision = 0.5, uint Color = 0)
        {
            double Length = Math.Sqrt(Math.Pow(X0 - X1, 2) + Math.Pow(Y0 - Y1, 2));

            double XStep = (X1 - X0) / (Length / Precision);
            double YStep = (Y1 - Y0) / (Length / Precision);

            double XRun = X0;
            double YRun = Y0;
            for (double i = 0; i < Length; i += Precision)
            {
                XRun += XStep;
                YRun += YStep;

                SetPixel(XRun, YRun, Color);
            }
        }

        private static double ToRadian(double Angle)
        {
            return Angle * (Math.PI / 180);
        }

        private void SetPixel(double X, double Y, uint Color)
        {
            int Pixel = GetPixel(X, Y);
            if (Pixel < 0)
                return;

            Buffer[Pixel] = Color;
        }

        private int GetPixel(double X, double Y)
        {
            int Pixel = ((int)Math.Round(Y) * Width) + (int)Math.Round(X);
            if (Pixel > Buffer.Length)
                return -1;

            if (X < 0)
                return -1;
            else if (X > Width)
                return -1;

            return Pixel;
        }
        public void Write()
        {
            using (FileStream file = new FileStream(OutputName, FileMode.Create, FileAccess.Write))
            {
                file.Write(new byte[] { 0x42, 0x4D }); // BM
                file.Write(BitConverter.GetBytes(Height * Width * sizeof(uint) + 0x1A)); // Size
                file.Write(BitConverter.GetBytes(0)); // Reserved (0s)
                file.Write(BitConverter.GetBytes(0x1A)); // Image Offset (size of the header)

                file.Write(BitConverter.GetBytes(0x0C)); // Header size (size is 12 bytes)
                file.Write(BitConverter.GetBytes(Width)); // Width
                file.Write(BitConverter.GetBytes(Height)); // Height
                file.Write(BitConverter.GetBytes((ushort)1)); // Color plane
                file.Write(BitConverter.GetBytes((ushort)32)); // bits per pixel

                byte[] Converted = new byte[Buffer.Length * sizeof(uint)];

                System.Buffer.BlockCopy(Buffer, 0, Converted, 0, Converted.Length);

                file.Write(Converted);
                file.Close();
            }
        }

        private readonly uint[] Buffer;
        private readonly ushort Width;
        private readonly ushort Height;
        private readonly string OutputName;
    }
}


