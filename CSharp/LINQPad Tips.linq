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

	var foo = Enumerable.Repeat(new OpenTickets(), 3);
	var foo2 = new { name = "Joe", Age = "21" };
	Console.WriteLine(foo2);
	Util.OnDemand("\r\nDeclare SQL Variables", () => "DECLARE @age INT = 21").Dump();
	Util.OnDemand("\r\nOpen Tickets", () => foo).Dump();
	Util.OnDemand("\r\nAll Tickets", () => foo2).Dump();
	
	Util.OnDemand("\r\nViews", () => GetViews()).Dump();
	Util.OnDemand("\r\nTables", () => "list of tables").Dump();
	Util.OnDemand("\r\nProcs", () => GetProcs()).Dump();

	//Util.DisplayWebPage("www.google.com");
//	var foo = new { name = "Joe", Age = "21" };
//	var bar = new { name = "Susan", Age = "21" };
//	Util.Dif(foo, bar).Dump();
//
//	var foo2 = new { name = "Joe", Age = "21" };
//	Util.OnDemand("Some Expensive Query", () => foo2).Dump();
//
//	// Util.OnDemand does the same thing, except it lets you specify the text to display in the hyperlink:
//	Util.OnDemand("Click me", () => 123).Dump();
//
//	// Util.OnDemand is useful for objects that are expensive to evaluate (or side-effecting). OnDemand is
//	// also exposed as an extension method on IEnumerable<T>:
//	"the quick brown fox".Split().OnDemand("Quick Fox").Dump();
//
	var answer = Util.ReadLine("Name", "Mark", new[] {"Joe", "Dave", "Rich"});
	Clipboard.SetText("You choose "+ answer, TextDataFormat.Text);
	answer.Dump("You choose "+ answer);

	//string[] output = Util.Cmd (@"dir", "/od");
}

class OpenTickets
{
	public string Name { get; set; }
	public int Age { get; set; }
}

object GetProcs()
{
	for (int i = 0; i < 3; i++)
	{
		var x = i;
		Util.OnDemand("Proc "+ i, () => x).Dump();
	}
	
	return "Procs";
}

object GetViews()
{
	var foo = new { name = "Joe", Age = "21" };

	for (int i = 0; i < 3; i++)
	{
		Util.OnDemand("View " + i, () => foo).Dump();
	}
	return "Views";
}


// Define other methods and classes here