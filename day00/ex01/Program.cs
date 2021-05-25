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
					if (s1[i - 1] == s2[j - 1])
						diff = 0;
					else
						diff = 1;
					m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + diff);
					j++;
				}
				i++;
			}

			return m[s1.Length, s2.Length];
		}

		static int CountNeededLetters(string name)
		{
			int i;
			int len;
			i = 0;
			len = name.Length;
			while (name[i] == ' ')
				i++;
			while (name[len - 1] == ' ')
				len--;
			return (len - i);
		}

		static string StrTrim(string name, int count_needed_letters)
		{
			string new_name;
			int i;
			int len;
			i = 0;
			len = name.Length;
			while (name[i] == ' ')
				i++;
			while (name[len - 1] == ' ')
				len--;
			new_name = name.Substring(i, len - i);
			Console.WriteLine(new_name);
			return (new_name);
		}
		static void Main(string[] args)
		{
			int i;
			int check;
			int distance_levenstein;
			int count_needed_letters;
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

			count_needed_letters = CountNeededLetters(name);
			name = StrTrim(name, count_needed_letters);
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
						while (button != "Y" && button != "N")
						{
							Console.WriteLine("Did you mean \"" + line[i] + "\"Y/N");
							button = Console.ReadLine();
						}
						if (button == "Y")
						{
							Console.WriteLine("Hello, " + line[i] + "!");
							return;
						}
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