<Query Kind="Program">
  <Reference>C:\tfs\Dev1\packages\Dapper.StrongName.1.50.5\lib\net451\Dapper.StrongName.dll</Reference>
  <Reference Relative="..\..\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll">C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\FastMember.Signed.dll</Reference>
  <Reference Relative="..\..\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll">C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Reference>C:\tfs\Dev1\packages\morelinq.2.10.0\lib\net40\MoreLinq.dll</Reference>
  <Reference>C:\Users\M755411\.nuget\packages\Newtonsoft.Json\11.0.2\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\PLDM.DataAccess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	//TODO: Create snippets for each of these
	var nums = Enumerable.Range(1, 6);
	var letters = new string[] { "A", "B", "C" };
	var months = new[] { 10, 20 };

//	var nums = Enumerable.Range(1, 6);
//	var letters = new string[] { "A", "B", "C" };
//	var months = new [] { 10, 20 };

	//nums.Permutations().Dump();
	letters.Permutations().Dump();
	//nums.Cartesian(letters, (i, letter) => letter + i.ToString()).Dump();
	//	nums.Cartesian(letters, (i, letter) => (letter, i))
	//		.Cartesian(months, (value, month) => (value.letter, value.i, month))
	//	.Dump();	

	//another Cartesian method
	//	var result = nums.SelectMany(n => letters, 
	//		(num, letter) => (Letter:letter, Number:num))
	//		.ToArray()
	//		.Dump();

	//var r = nums.Lead(2, (curr, lag) => (curr, lag)).Dump(); ;
	var r = letters.Lead(1, (curr, lag) => (curr, lag)).Dump(); ;

}

// Define other methods and classes here