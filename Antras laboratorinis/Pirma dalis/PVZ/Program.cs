using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Pirma
{
	class Program
	{
		static void Main(string[] args)
		{
			int size = 2000;
			int n = 10;
			long k;
			Console.WriteLine("Pirma rekurentine: ");
			Console.WriteLine("1 - arr[0] = 0");
			for (int i = 0; i < n; i++)
			{
				Stopwatch stopwatch = new Stopwatch();
				int[] A = new int[size];
				A[0] = 0;
				stopwatch.Start();
				k = methodToAnalysis1(A);
				stopwatch.Stop();
				Console.WriteLine($"Function working time: {stopwatch.Elapsed}");
				Console.WriteLine($"Elements amount: {size}");
				Console.WriteLine($"Counter: {k}");
				Console.WriteLine();
				GC.Collect();
				size += 2000;
			}

			size = 20;
			Console.WriteLine("Pirma rekurentine: ");
			Console.WriteLine("1 - arr[0] = 1");
			for (int i = 0; i < n; i++)
			{
				Stopwatch stopwatch = new Stopwatch();
				int[] A = new int[size];
				A[0] = -1;
				stopwatch.Start();
				k = methodToAnalysis1(A);
				stopwatch.Stop();
				Console.WriteLine($"Function working time: {stopwatch.Elapsed}");
				Console.WriteLine($"Elements amount: {size}");
				Console.WriteLine($"Counter: {k}");
				Console.WriteLine();
				GC.Collect();
				size += 20;
			}

			//n = 8;
			//size = 2000;
			//Console.WriteLine("Antra rekurentine:");
			//for (int i = 0; i < n; i++)
			//{
			//	Stopwatch stopwatch = new Stopwatch();
			//	stopwatch.Start();
			//	k = methodToAnalysis2(size);
			//	stopwatch.Stop();
			//	Console.WriteLine($"Function working time: {stopwatch.Elapsed}");
			//	Console.WriteLine($"Elements amount: {size}");
			//	Console.WriteLine($"Counter: {k}");
			//	Console.WriteLine();
			//	GC.Collect();
			//	size += 2000;
			//}
		}

		//Pirma rekurentine
		public static long methodToAnalysis1(int[] arr)
		{
			long n = arr.Length;
			long k = n;

			if (arr[0] < 0)
			{
				for (int i = 0; i < n; i++)
				{
					if (i > 0)
					{
						for (int j = 0; j < n; j++)
						{
							k -= 2;
						}
					}
				}
			}
			return k;
		}

		//Antra rekurentine
		public static long methodToAnalysis2(int n)

		{

			long k = 0;
			int[] arr = new int[n];
			Random randNum = new Random();
			for (int i = 0; i < n; i++)
			{
				arr[i] = randNum.Next(0, n);
				k += arr[i] + FF1(i);
			}
			return k;

		}

		public static long FF1(int n)

		{
			if (n > 0)
			{
				return FF1(n - 1);
			}
			return n;
		}
	}
}
