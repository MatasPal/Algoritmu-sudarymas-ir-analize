using System;
using System.Collections.Generic;
using System.IO;

using System;
using System.Collections.Generic;

public class City
{
	public string Name { get; set; }
	public long ID { get; set; }
	public double X { get; set; }
	public double Y { get; set; }
}

public class TravelingSalesmanProblem
{
	private List<City> cities;
	private int numberOfCities;

	public TravelingSalesmanProblem(List<City> cities)
	{
		this.cities = cities;
		numberOfCities = cities.Count;
	}

	public List<int> FindShortestRoute()
	{
		int[,] distanceMatrix = CalculateDistanceMatrix();

		List<int> route = new List<int>();
		bool[] visited = new bool[numberOfCities];

		// Start with the first city
		int currentCity = 0;
		route.Add(currentCity);
		visited[currentCity] = true;

		while (route.Count < numberOfCities)
		{
			int nearestCity = FindNearestNeighbor(currentCity, visited, distanceMatrix);
			route.Add(nearestCity);
			visited[nearestCity] = true;
			currentCity = nearestCity;
		}

		// Return to the starting city
		route.Add(0);

		return route;
	}

	private int[,] CalculateDistanceMatrix()
	{
		int[,] distanceMatrix = new int[numberOfCities, numberOfCities];

		for (int i = 0; i < numberOfCities; i++)
		{
			for (int j = 0; j < numberOfCities; j++)
			{
				distanceMatrix[i, j] = CalculateDistance(cities[i], cities[j]);
			}
		}

		return distanceMatrix;
	}

	private int CalculateDistance(City city1, City city2)
	{
		double deltaX = city1.X - city2.X;
		double deltaY = city1.Y - city2.Y;
		double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
		return (int)Math.Round(distance);
	}

	private int FindNearestNeighbor(int city, bool[] visited, int[,] distanceMatrix)
	{
		int nearestCity = -1;
		int shortestDistance = int.MaxValue;

		for (int i = 0; i < numberOfCities; i++)
		{
			if (i != city && !visited[i] && distanceMatrix[city, i] < shortestDistance)
			{
				shortestDistance = distanceMatrix[city, i];
				nearestCity = i;
			}
		}

		return nearestCity;
	}
}


public class Program
{
	public static void Main()
	{
		List<City> cities = new List<City>();

		// Read data from a file
		using (StreamReader reader = new StreamReader("Data/Table1.csv"))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				string[] parts = line.Split(',');
				if (parts.Length == 4)
				{
					City city = new City
					{
						Name = parts[0],
						ID = long.Parse(parts[1]),
						X = double.Parse(parts[2]),
						Y = double.Parse(parts[3])
					};
					cities.Add(city);
				}
			}
		}

		TravelingSalesmanProblem tsp = new TravelingSalesmanProblem(cities);
		List<int> shortestRoute = tsp.FindShortestRoute();

		Console.WriteLine("Shortest route:");
		foreach (int cityIndex in shortestRoute)
		{
			City city = cities[cityIndex];
			Console.WriteLine($"Name: {city.Name}, ID: {city.ID}, X: {city.X}, Y: {city.Y}");
		}
	}
}


//public class TravelingSalesmanProblem
//{
//	private int[,] distanceMatrix;
//	private int numberOfCities;

//	public TravelingSalesmanProblem(int[,] distances)
//	{
//		distanceMatrix = distances;
//		numberOfCities = distanceMatrix.GetLength(0);
//	}

//	public List<int> FindShortestRoute()
//	{
//		List<int> route = new List<int>();
//		bool[] visited = new bool[numberOfCities];

//		// Start with the first city
//		int currentCity = 0;
//		route.Add(currentCity);
//		visited[currentCity] = true;

//		while (route.Count < numberOfCities)
//		{
//			int nearestCity = FindNearestNeighbor(currentCity, visited);
//			route.Add(nearestCity);
//			visited[nearestCity] = true;
//			currentCity = nearestCity;
//		}

//		// Return to the starting city
//		route.Add(0);

//		return route;
//	}

//	private int FindNearestNeighbor(int city, bool[] visited)
//	{
//		int nearestCity = -1;
//		int shortestDistance = int.MaxValue;

//		for (int i = 0; i < numberOfCities; i++)
//		{
//			if (i != city && !visited[i] && distanceMatrix[city, i] < shortestDistance)
//			{
//				shortestDistance = distanceMatrix[city, i];
//				nearestCity = i;
//			}
//		}

//		return nearestCity;
//	}
//}
/*
public class Program
{
	public static void Main()
	{
		// Example usage
		int[,] distanceMatrix = {
			{0, 10, 15, 20},
			{10, 0, 35, 25},
			{15, 35, 0, 30},
			{20, 25, 30, 0}
		};

		TravelingSalesmanProblem tsp = new TravelingSalesmanProblem(distanceMatrix);
		List<int> shortestRoute = tsp.FindShortestRoute();

		Console.WriteLine("Shortest route:");
		foreach (int city in shortestRoute)
		{
			Console.Write(city + " ");
		}
	}
}
*/