using System;
using System.Text.RegularExpressions;
using System.IO;

namespace DictionarySearchName
{
	class Program
	{
		static int LevenshteinDistance(string s1, string s2)
		{
			int diff;
			int[,] m = new int[s1.Length + 1, s2.Length + 1];
			int i = 0;
			int j = 0;
			while (i <= s1.Length)
			{
				m[i, 0] = i;
				i++;
			}

			while (j <= s2.Length)
			{
				m[0, j] = j;
				j++;
			}

			i = 1;
			while (i <= s1.Length)
			{
				j = 1;
				while (j <= s2.Length)
				{
					diff = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
					m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + diff);
					j++;
				}

				i++;
			}

			return m[s1.Length, s2.Length];
		}

		static void Main(string[] args)
		{
			int i;
			int check;
			int distance_levenstein;
			string button;
			string name;
			string model_name;
			string[] line = File.ReadAllLines("us.txt");
			Console.WriteLine("Enter name:");
			name = Console.ReadLine();
			check = 0;
			model_name = @"^[A-Za-z -]+$";
			if (!Regex.IsMatch(name, model_name))
			{
				Console.WriteLine("Your name was not found.");
				return;
			}
			while (check < 3)
			{
				i = 0;
				while (i < line.Length)
				{
					distance_levenstein = LevenshteinDistance(name, line[i]);
					if (distance_levenstein == check)
					{
						if (distance_levenstein == 0)
						{
							Console.WriteLine("Hello, " + line[i] + "!");
							return;
						}
						Console.WriteLine("Did you mean \"" + line[i] + "\"Y/N");
						button = Console.ReadLine();
						if (button == "Y")
						{
							Console.WriteLine("Hello, " + line[i] + "!");
							return;
						}
						else if (button != "N")
							return;
					}
					i++;
				}
				check++;
			}
			Console.WriteLine("Your name was not found.");
			return;
		}
	}
}