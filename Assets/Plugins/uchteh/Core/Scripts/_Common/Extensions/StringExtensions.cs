using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core.Extensions
{
	public static class StringExtensions
	{
		public static List<string> Digits = new List<string>
		{
		"Zero",
		"One",
		"Two",
		"Three",
		"Four",
		"Five",
		"Six",
		"Seven",
		"Eight",
		"Nine"

	};

		public static List<int> GetNumberList(this string stringWithDigits)
		{
			List<int> result = new List<int>();

			Dictionary<string, int> myDictionary = new Dictionary<string, int>();

			for (int i = 0; i < Digits.Count; i++)
			{
				myDictionary.Add(Digits[i], i);

			}

			foreach (var strDig in Regex.Split(stringWithDigits, @"(?<!^)(?=[A-Z])"))
			{
				result.Add(myDictionary[strDig]);
			}
			return result;
		}

		public static int ConvertToDigit(List<int> digits)
		{
			int result = 0;
			for (int i = 0; i < digits.Count; i++)
			{
				result += (int)Math.Pow(10, digits.Count - 1 - i) * digits[i];
			}

			Debug.Log("Convert " + result);
			return result;
		}

		public static int CompareLists(List<int> oneString, List<int> twoString)
		{
			if (ConvertToDigit(oneString) > ConvertToDigit(twoString))
			{
				return 1;
			}
			if (ConvertToDigit(oneString) < ConvertToDigit(twoString))
			{
				return -1;
			}

			return 0;
		}
	}
}
