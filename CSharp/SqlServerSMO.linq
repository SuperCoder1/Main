<Query Kind="Program">
  <Reference>C:\tfs\Dev1\packages\Dapper.StrongName.1.50.5\lib\net451\Dapper.StrongName.dll</Reference>
  <Reference Relative="..\..\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll">C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\FastMember.Signed.dll</Reference>
  <Reference Relative="..\..\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll">C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Reference>C:\Users\M755411\.nuget\packages\Newtonsoft.Json\11.0.2\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\PLDM.DataAccess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>Microsoft.SqlServer.SqlManagementObjects</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Common</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>Microsoft.SqlServer.Management.Smo.Agent</Namespace>
</Query>

void Main()
{
	//TODO: Use this API to backup objs by generating create or alter scripts for db objs
	//TODO: Encapsulate this in a class w/IDisposable to disconnect

	//	ScriptObjects("Scripting Table & Statistics: ", tso,
	//		db.Tables.Cast<Table>().Where(t => !t.IsSystemObject));

	//	ScriptObjects("Scripting Stored Procedure: ", tso,
	//		db.StoredProcedures.Cast<StoredProcedure>().Where(t => !t.IsSystemObject));

	//spetl_GetClientID
	//spetl_EnableAllIndexes
	var sp = db.StoredProcedures["spetl_GetClientID", "dbo"];
	//	sp.TextHeader.Dump("Header");
	//	sp.TextBody.Dump("Body");
	sp.Parameters[0].Name.Dump();
	sp.Parameters[0].DataType.Dump();
	sp.Parameters[1].Name.Dump();
	sp.Parameters[1].DataType.Dump();

	//	var filter = new JobHistoryFilter();
	//	filter.StartRunDate = DateTime.Today.AddDays(-2);
	//	filter.EndRunDate = DateTime.Now;

	//server.JobServer.Jobs[0].EnumHistory(filter);

	foreach (PartitionFunction pf in db.PartitionFunctions)
	{
		Console.WriteLine("Scripting Partition Function: " + pf);
		pf.Script(tso);
	}

	foreach (PartitionScheme ps in db.PartitionSchemes)
	{
		Console.WriteLine("Scripting Partition Scheme: " + ps);
		ps.Script(tso);
	}

	Console.Write("Scripting completed. Press any key to exit.");
	Console.ReadKey();

}

// Define other methods and classes here
public class SqlServerSMO : IDisposable
{
	public Server Server { get; }
	public Database Database { get; }
	private ServerConnection _conn;
	private ScriptingOptions _scriptingOpts = null; 

	public SqlServerSMO(string serverName, string databaseName)
	{
		if (string.IsNullOrWhiteSpace(serverName))
			throw new ArgumentNullException(nameof(serverName));

		if (string.IsNullOrWhiteSpace(databaseName))
			throw new ArgumentNullException(nameof(databaseName));

		Server = new Server(serverName);
		Database = Server.Databases[databaseName];

		_conn = new ServerConnection(serverName);
		_conn.Connect();
	}

	//		tso.FileName = "c:\\" + databasename + "-clone.sql";
	public void ScriptDatabase()
	{
		//include the database create syntax
		//	ScriptingOptions dbso = new ScriptingOptions();
		//	dbso.FileName = "e:\\" + databasename + "-create.sql";
		//	dbso.AppendToFile = true;
		//
		//	Console.WriteLine("Scripting database: " + databasename + ". Please wait...");
		//
		//	db.Script(dbso);
	}

	public void ScriptFunctions(string consoleMsg
		Predicate<UserDefinedFunction> predicate) 
	{
		var objs = Database.UserDefinedFunctions.Cast<UserDefinedFunction>()
				.Where(obj => predicate(obj) && !obj.IsSystemObject);
		ScriptObjects(consoleMsg, objs);
		
	}

	public void ScriptStoredProcedures(string consoleMsg
		Predicate<StoredProcedure> predicate)
	{
		var objs = Database.StoredProcedures.Cast<StoredProcedure>()
				.Where(obj => predicate(obj) && !obj.IsSystemObject);
		ScriptObjects(consoleMsg, objs);
	}

	private void ScriptObjects<T>(string consoleMsg,
		IEnumerable<T> objs) where T : IScriptable
	{
		foreach (T obj in objs)
		{
			Console.WriteLine(consoleMsg + obj);
			obj.Script(_scriptingOpts);
		}
	}

	private void InitScriptingOptions()
	{
		ScriptingOptions tso = new ScriptingOptions();
		tso.ScriptDrops = false;
		tso.Indexes = true;
		tso.ClusteredIndexes = true;
		tso.PrimaryObject = true;
		tso.SchemaQualify = true;
		tso.NoIndexPartitioningSchemes = false;
		tso.NoFileGroup = false;
		tso.DriPrimaryKey = true;
		tso.DriChecks = true;
		tso.DriAllKeys = true;
		tso.AllowSystemObjects = false;
		tso.IncludeIfNotExists = false;
		tso.DriForeignKeys = true;
		tso.DriAllConstraints = true;
		tso.DriIncludeSystemNames = true;
		tso.AnsiPadding = true;
		tso.IncludeDatabaseContext = false;
		tso.AppendToFile = true;

		//include statistics and histogram data for db clone
		tso.OptimizerData = true;
		tso.Statistics = true;
	}

	public void Dispose()
	{
		_conn.Disconnect();
	}
}
