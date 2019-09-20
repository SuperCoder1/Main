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
	
}

//using System;
//using System.Collections;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using System.Text.RegularExpressions;

/// <summary>
/// This class can be used to execute SQL scripts on a database.
/// It takes a SQL script, parsers out the GO's and executes each 
/// batch remembering the correct line numbers.
/// </summary>
public class ScriptUtility
{
	private Regex batchRegex = new Regex(@"(?<=^)[ \t]*\bGO\b[ \t\r]*\n?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
	private Regex lineRegex = new Regex(@"\r?\n", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

	public event SqlInfoMessageEventHandler ScriptInfoMessage;

	private string _connectionString;
	private string _script;

	public ScriptUtility()
	{
	}

	public ScriptUtility(string connectionString, string script)
	{
		_connectionString = connectionString;
		_script = script;
	}

	/// <summary>
	/// Executes a SQL script using the given connection string.
	/// </summary>
	/// <param name="connectionString"></param>
	/// <param name="script"></param>
	/// <returns></returns>
	public static ScriptResult ExecuteScript(string connectionString, string script)
	{
		ScriptUtility utility = new ScriptUtility(connectionString, script);
		return utility.ExecuteScript();
	}

	/// <summary>
	/// Executes a SQL script using the given connection string.
	/// </summary>
	/// <param name="connectionString"></param>
	/// <param name="script"></param>
	/// <param name="infoMessageHandler"></param>
	/// <returns></returns>
	public static ScriptResult ExecuteScript(string connectionString, string script, SqlInfoMessageEventHandler infoMessageHandler)
	{
		ScriptUtility utility = new ScriptUtility(connectionString, script);
		utility.ScriptInfoMessage += infoMessageHandler;
		return utility.ExecuteScript();
	}

	/// <summary>
	/// The connection string to use when executing the script.
	/// </summary>
	public string ConnectionString
	{
		get { return _connectionString; }
		set { _connectionString = value; }
	}

	/// <summary>
	/// The script to execute.
	/// </summary>
	public string Script
	{
		get { return _script; }
		set { _script = value; }
	}

	/// <summary>
	/// Executes the script.
	/// </summary>
	/// <returns></returns>
	public ScriptResult ExecuteScript()
	{
		Batch[] batches = SplitScript(this.Script);
		var errors = new List<ScriptError>();
		bool success = false;

		using (SqlConnection cn = new SqlConnection(this.ConnectionString))
		{
			cn.InfoMessage += new SqlInfoMessageEventHandler(cn_InfoMessage);

			for (int i = 0; i < batches.Length; i++)
			{
				Batch currentBatch = batches[i];

				using (IDbCommand cmd = cn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = batches[i].Content;
					cmd.Connection = cn;

					if (cn.State != ConnectionState.Open) cn.Open();
					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (SqlException e)
					{
						for (int x = 0; x < e.Errors.Count; x++)
						{
							errors.Add(new ScriptError(e.Errors[x].Number, e.Errors[x].State, e.Errors[x].Class, e.Errors[x].Message, e.Errors[x].Procedure, e.Errors[x].LineNumber + currentBatch.StartLineNumber - 1));
						}
					}
				}
			}
		}

		if (errors.Count == 0) success = true;

		return new ScriptResult(success, errors);
	}

	private void cn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
	{
		if (this.ScriptInfoMessage != null)
		{
			this.ScriptInfoMessage(sender, e);
		}
	}

	private Batch[] SplitScript(string script)
	{
		ArrayList batchList = new ArrayList();
		MatchCollection batchMatches = batchRegex.Matches(script);

		int currentIndex = 0;
		int currentLine = 1;
		for (int i = 0; i < batchMatches.Count; i++)
		{
			string content = script.Substring(currentIndex, batchMatches[i].Index - currentIndex);
			int lineCount = lineRegex.Matches(content).Count;

			if (content.Trim().Length > 0)
			{
				Batch batch = new Batch(currentLine, lineCount, content);
				batchList.Add(batch);
			}

			currentIndex = batchMatches[i].Index + batchMatches[i].Length;
			currentLine += lineCount + 1;
		}

		if (currentIndex < script.Length - 1)
		{
			string content = script.Substring(currentIndex);
			int lineCount = lineRegex.Matches(content).Count;
			Batch batch = new Batch(currentLine, lineCount, content);
			batchList.Add(batch);
		}

		return (Batch[])batchList.ToArray(typeof(Batch));
	}
}

/// <summary>
/// The result of a script execution including any errors that may have occurred.
/// </summary>
public class ScriptResult
{
	public ScriptResult(bool success, ScriptErrorCollection errors)
	{
		Success = success;
		Errors = errors;
	}

	/// <summary>
	/// Whether the script was executed successfully or not.
	/// </summary>
	public bool Success { get; }

	/// <summary>
	/// Any errors that occurred as the result of executing the script.
	/// </summary>
	public IList<ScriptError> Errors { get; }

	/// <summary>
	/// Returns a string containing the script execution results including errors.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		if (Success)
			return "Script executed successfully.";

		var builder = new StringBuilder();
		builder.Append("Script execution failed with the following errors:\r\n\r\n");

		foreach (ScriptError error in Errors)
			builder.AppendFormat("{0}\r\n", error);

		return builder.ToString();
	}
}