using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace LIS
{
	interface ILis
	{
		// String containing original sequence of numbers and whitespaces.
		String InputString { get; set; }
		// Calculates longest increasing subsequence (increased by any number) from a given string of numbers and whitespaces.
		String GetLis();
	}

	public class Program
	{
		public static void Main()
		{
			Lis.PrintLis();
		}
	}

	//Class responsible for taking a sequence from user and calculating Longest Increasing Subsequence out of it.
	public class Lis : ILis
	{
		private static String _inputString;

		//Get original input sequence
		public String InputString
		{
			get
			{
				return _inputString;
			}
			set
			{
				Regex rx = new Regex("^[0-9 ]+$");
				_inputString = rx.IsMatch(value) ? value : String.Empty; //Check if string has got anything other than integers and whitespaces
			}
		}

		//Take the input from user and print the output to console
		public static void PrintLis()
		{
			try
			{
				//Lis class will return longest increasing subsequence through its GetLis method.
				ILis lis = new Lis();

				Console.WriteLine("\n\nPlease input the original sequence containing only numbers and whitespaces and press enter: ");

				// We need a large buffer since default one is too small for those large inputs.
				byte[] inputBuffer = new byte[1024 * 30];
				Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
				Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
				lis.InputString = Console.ReadLine();

				if (String.IsNullOrEmpty(lis.InputString))
					Console.WriteLine("\n\nIncorrect string format!");
				else
					Console.WriteLine("\n\nThe Longest Increasing Subsequence is: {0}", lis.GetLis());

				Console.ReadKey();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Something went wrong!");
			}
			finally
			{
				GC.Collect();
			}
		}

		// Calculates longest increasing subsequence (increased by any number) from a given string of numbers and whitespaces.
		public String GetLis()
		{
			int[] originalSeq = _inputString.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().Select(int.Parse).ToArray(); //Original sequence
			IList<int> x = new List<int>(); //Carrier of current subsequence
			IList<int> y = new List<int>(); //LIS/Longest increasing subsequence
			int n = originalSeq.Length; // lenght  of input array/stringa
			int max; //Current max

			if (n != 0 && n > 1) //if we need to find LIS at all
			{
				for (int i = 0, k; i < n; i++) //Traverse through whole sequence
				{
					max = originalSeq[i]; //max of current subsequence
					x.Clear(); //Get x ready to absorb new subsequence
					x.Add(max); //Build up carrier subsequence
					k = i + 1; //To loop through Every possible subsequence for current item of original sequence

					for (int j = k; j < n; j++) // Check all subsequences for current item of original sequence
					{
						if (originalSeq[j] > max)
						{
							x.Add(max = originalSeq[j]); // Add new max to carrier LIS and reassign max with it
						}

						if (j == n - 1 && k != n - 1) // Reset loop for current item in original sequence to one next item in sequence
						{
							k++;
							j = k; //Start with next item for find more possible subsequences for ith element of original sequence

							if (x.Count > y.Count) // If carrier subsequences is longer than result LIS, update result LIS with new subsequence
							{
								y.Clear();
								y = x.ToList();
							}

							x.Clear();
							x.Add(max = originalSeq[i]); // Reset carrier with current item in original sequence	
						}

					}
				}
			}
			else
				y.Add(originalSeq[n - 1]); //Where original sequence has only one element

			return String.Join(" ", y);
		}
	}
}
