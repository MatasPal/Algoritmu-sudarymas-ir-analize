
//Recursion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

class TreasureDivisionRecursion
{
	static int[] values;
	static bool[] selected;
	static List<int> firstBrother, secondBrother;
	static int minDifference = int.MaxValue;

	/// <summary>
	/// Recursive solution
	/// </summary>
	/// <param name="index"></param>
	/// <param name="sum1"></param>
	/// <param name="sum2"></param>
	static void CanDivide(int index, int sum1, int sum2)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		if (index == values.Length)
		{
			int difference = Math.Abs(sum1 - sum2);
			if (difference < minDifference)
			{
				minDifference = difference;
				firstBrother.Clear();
				secondBrother.Clear();

				for (int i = 0; i < selected.Length; i++)
				{
					if (selected[i])
					{
						firstBrother.Add(values[i]);
					}
					else
					{
						secondBrother.Add(values[i]);
					}
				}
			}
			return;
		}

		// Try adding the item to the first brother's share
		selected[index] = true;
		CanDivide(index + 1, sum1 + values[index], sum2);

		// Try adding the item to the second brother's share
		selected[index] = false;
		CanDivide(index + 1, sum1, sum2 + values[index]);
		stopwatch.Stop();
		Console.WriteLine($"Function working time: {stopwatch.Elapsed}");
		Console.WriteLine();
	}

	static void Main()
	{
		values = new int[] { 2, 4, 5, 3 };
		selected = new bool[values.Length];
		firstBrother = new List<int>();
		secondBrother = new List<int>();

		CanDivide(0, 0, 0);

		if (minDifference == 0)
		{
			Console.WriteLine("The treasure can be divided equally:");
		}
		else
		{
			Console.WriteLine("The treasure cannot be divided equally, but can be divided similarly:");
		}
		Console.WriteLine("First brother's share: " + string.Join(", ", firstBrother));
		Console.WriteLine("Second brother's share: " + string.Join(", ", secondBrother));
	}
}

//Dynamic
//using System;

//public class TreasureDivision
//{
//	public static bool CanDivide(int[] items)
//	{
//		int n = items.Length;
//		int S = 0;
//		for (int i = 0; i < n; i++)
//		{
//			S += items[i];
//		}
//		if (S % 2 != 0)
//		{
//			// The total sum is odd, so the items cannot be divided into two groups with equal sums.
//			return false;
//		}
//		bool[,] DP = new bool[n + 1, S / 2 + 1];
//		// Initialize the base case where the sum is zero.
//		for (int i = 0; i <= n; i++)
//		{
//			DP[i, 0] = true;
//		}
//		// Compute the remaining cases.
//		for (int i = 1; i <= n; i++)
//		{
//			for (int j = 1; j <= S / 2; j++)
//			{
//				if (items[i - 1] > j)
//				{
//					DP[i, j] = DP[i - 1, j];
//				}
//				else
//				{
//					DP[i, j] = DP[i - 1, j] || DP[i - 1, j - items[i - 1]];
//				}
//			}
//		}
//		return DP[n, S / 2];
//	}

//	public static void Main(string[] args)
//	{
//		int[] items = new int[] { 2, 4, 5, 6, 7 };
//		if (CanDivide(items))
//		{
//			Console.WriteLine("The items can be divided into two groups with equal sums.");
//		}
//		else
//		{
//			Console.WriteLine("The items cannot be divided into two groups with equal sums.");
//		}
//	}
//}
