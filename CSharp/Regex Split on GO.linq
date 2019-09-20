<Query Kind="Program">
  <Reference>C:\tfs\Dev1\packages\Dapper.StrongName.1.50.5\lib\net451\Dapper.StrongName.dll</Reference>
  <Reference Relative="..\..\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll">C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\FastMember.Signed.dll</Reference>
  <Reference Relative="..\..\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll">C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Reference>C:\Users\M755411\.nuget\packages\Newtonsoft.Json\11.0.2\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\PLDM.DataAccess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
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
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	
	
	const string input = @"
-- comment1
SELECT 1

GO

-- comment2
SELECT 2
SELECT 'X'
GO";
	var split =
	Regex.Split(input, @"[ \t]*\bGO\b[ \t\r]*\n?$", RegexOptions.IgnoreCase
	//Regex.Split(input, @"(?<=^)[ \t]*\bGO\b[ \t\r]*\n?$", RegexOptions.IgnoreCase
		| RegexOptions.Multiline
		| RegexOptions.ExplicitCapture);
		
	split.Dump();
}

// Define other methods and classes here
