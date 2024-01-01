<Query Kind="Program">
  <Namespace>System.IO</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	string jsonFilePath = @"C:\Users\deepi\OneDrive\Documents\LINQPad Queries\Allscripts\Employee.json";
	List<ColumnInfo> columnInfoList = LoadColumnInfoFromJson(jsonFilePath);

	foreach (ColumnInfo columnInfo in columnInfoList)
	{
	    Console.WriteLine($"Column Name: {columnInfo.ColumnName}, Data Type: {columnInfo.DataType}");
	}	
}

// Define other methods and classes here
public class ColumnInfo
{
    public string ColumnName { get; set; }
    public string DataType { get; set; }
}

public static List<ColumnInfo> LoadColumnInfoFromJson(string jsonFilePath)
{
    string jsonString = File.ReadAllText(jsonFilePath);
    List<ColumnInfo> columnInfoList = JsonSerializer.Deserialize<List<ColumnInfo>>(jsonString);
    return columnInfoList;
}
