using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleREngine
{
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

		public void DrawFilledCircle(double X, double Y, double R, double Precision = 0.5, uint Color = 0)
		{
			for (double i = 90; i < 270; i += Precision)
			{
				double Angle = ToRadian(i);

				double CX = (Math.Cos(Angle) * R) + X;
				double CY = (Math.Sin(Angle) * R) + Y;

				double EX = (Math.Cos(Math.PI - Angle) * R) + X;

				for (; CX < EX; CX++)
					SetPixel(CX, CY, Color);
			}
		}

		public void DrawCircle(double X, double Y, double R, double Precision = 0.5, uint Color = 0)
		{
			for (double i = 0; i < 360; i += Precision)
			{
				double Angle = ToRadian(i);

				double CX = (Math.Cos(Angle) * R) + X;
				double CY = (Math.Sin(Angle) * R) + Y;

				SetPixel(CX, CY, Color);
			}
		}

		public void DrawFilledSquare(double X, double Y, double Width, double Height, uint Color = 0)
		{
			for (double SY = 0; SY < Height; SY++)
			{
				for (double SX = 0; SX < Width; SX++)
				{
					SetPixel(SX + X, SY + Y, Color);
				}
			}
		}

		public void DrawSquare(double X, double Y, double Width, double Height, uint Color)
		{
			DrawLine(X, Y, X + Width, Y, 1, Color);
			DrawLine(X, Y, X, Y + Height, 1, Color);
			DrawLine(X + Width, Y, X + Width, Y + Height, 1, Color);
			DrawLine(X, Y + Height, X + Width, Y + Height, 1, Color);
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
            using (FileStream File = new FileStream(OutputName, FileMode.Create, FileAccess.Write))
            {
				File.Write(new byte[] { 0x42, 0x4D }); // BM
				File.Write(BitConverter.GetBytes(Height * Width * sizeof(uint) + 0x1A)); // Size
				File.Write(BitConverter.GetBytes(0)); // Reserved (0s)
				File.Write(BitConverter.GetBytes(0x1A)); // Image Offset (size of the header)

				File.Write(BitConverter.GetBytes(0x0C)); // Header size (size is 12 bytes)
				File.Write(BitConverter.GetBytes(Width)); // Width
				File.Write(BitConverter.GetBytes(Height)); // Height
				File.Write(BitConverter.GetBytes((ushort)1)); // Color plane
				File.Write(BitConverter.GetBytes((ushort)32)); // bits per pixel

				byte[] Converted = new byte[Buffer.Length * sizeof(uint)];

				System.Buffer.BlockCopy(Buffer, 0, Converted, 0, Converted.Length);

				File.Write(Converted);
				File.Close();
            }
        }

		private readonly uint[] Buffer;
		private readonly ushort Width;
		private readonly ushort Height;
		private readonly string OutputName;
	}
}
