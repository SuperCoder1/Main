public static class StringSearchExtensions
{

	//This is a case-sensitive search
	public static int IndexOfKMP(this string text, string pattern)
	{
		int N = text.Length;
		int M = pattern.Length;

		if (N < M) return -1;
		if (N == M && text == pattern) return 0;
		if (M == 0) return 0;

		int[] lpsArray = new int[M];

		LongestPrefixSuffix(pattern, ref lpsArray);

		int i = 0, j = 0;
		while (i < N)
		{
			if (text[i] == pattern[j])
			{
				i++;
				j++;
			}

			// match found at i-j
			if (j == M)
			{
				return (i - j);
			}
			else if (i < N && text[i] != pattern[j])
			{
				if (j != 0)
				{
					j = lpsArray[j - 1];
				}
				else
				{
					i++;
				}
			}
		}

		return -1;
	}

	//This is a case-sensitive search
	public static int[] IndexesOfKMP(this string text, string pattern)
	{
		int N = text.Length;
		int M = pattern.Length;

		if (N < M) return new int[] { -1 };
		if (N == M && text == pattern) return new int[] { 0 };
		if (M == 0) return new int[] { 0 };

		int[] lpsArray = new int[M];
		List<int> matchedIndex = new List<int>();

		LongestPrefixSuffix(pattern, ref lpsArray);

		int i = 0, j = 0;
		while (i < N)
		{
			if (text[i] == pattern[j])
			{
				i++;
				j++;
			}

			// match found at i-j
			if (j == M)
			{
				matchedIndex.Add(i - j);
				j = lpsArray[j - 1];
			}
			else if (i < N && text[i] != pattern[j])
			{
				if (j != 0)
				{
					j = lpsArray[j - 1];
				}
				else
				{
					i++;
				}
			}
		}

		return matchedIndex.ToArray();
	}

	private static void LongestPrefixSuffix(string pattern, ref int[] lpsArray)
	{
		int M = pattern.Length;
		int len = 0;
		lpsArray[0] = 0;
		int i = 1;

		while (i < M)
		{
			if (pattern[i] == pattern[len])
			{
				len++;
				lpsArray[i] = len;
				i++;
			}
			else
			{
				if (len == 0)
				{
					lpsArray[i] = 0;
					i++;
				}
				else
				{
					len = lpsArray[len - 1];
				}
			}
		}
	}
}
