<Query Kind="Program" />


//From: https://github.com/Humanizr/Humanizer
//src/Humanizer/InflectorExtensions.cs
void Main()
{
	"IOStream".ToUnderscoreCase().Dump();
	"IOStream".Pascalize().Dump();
	"IOStream".Camelize().Dump();
	"IOStream".Underscore().Dump();
	"IOStreamMyTeamWinsTheGame".Underscore().Dump("Underscore");
	
	"HIRE_DATE".ToUnderscoreCase().Dump();
	"HIRE_DATE".Pascalize().Dump("Pascalize");
	"HIRE_DATE".Camelize().Dump("Camelize 1");
	"hire_date".Camelize().Dump("Camelize 2");
	"EmployeeName".Camelize().Dump("Camelize 3");
	"Employee_Name".Camelize().Dump("Camelize 4");
	"HIRE_DATE".Underscore().Dump();	
}

public static class StringExtensions
{
	public static string ToUnderscoreCase(this string str) =>
	    string.Concat(
	        str.Select((x, i) =>
	            i > 0 && char.IsUpper(x) && (char.IsLower(str[i - 1]) || i < str.Length - 1 && char.IsLower(str[i + 1]))
	                ? "_" + x
	                : x.ToString())).ToLowerInvariant();

        /// <summary>
        /// Separates the input words with underscore
        /// </summary>
        /// <param name="input">The string to be underscored</param>
        /// <returns></returns>
        public static string Underscore(this string input)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(input, @"([\p{Lu}]+)([\p{Lu}][\p{Ll}])", "$1_$2"), @"([\p{Ll}\d])([\p{Lu}])", "$1_$2"), @"[-\s]", "_").ToLower();
        }
		
        /// <summary>
        /// By default, pascalize converts strings to UpperCamelCase also removing underscores
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Pascalize(this string input)
        {   
            return Regex.Replace(input, "(?:^|_| +)(.)", match => match.Groups[1].Value.ToUpper());
        }

        /// <summary>
        /// Same as Pascalize except that the first character is lower case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Camelize(this string input)
        {
            var word = input.Pascalize();
            return word.Length > 0 ? word.Substring(0, 1).ToLower() + word.Substring(1) : word;
        }
}
				
