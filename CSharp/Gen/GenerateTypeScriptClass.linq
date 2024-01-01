<Query Kind="Program">
  <Namespace>Snowflake.Client</Namespace>
</Query>

//Output (example):
//
//TypeScript
//export class Employee {
//    public employeeId: number;
//    public firstName: string;
//    public lastName: string;
//    public hireDate: Date;
//    // ... other properties
//}

void Main()
{
	string tsClass = GenerateTypeScriptClass("EMPLOYEE");
	Console.WriteLine(tsClass);
}

public static string GenerateTypeScriptClass(string tableName)
{
    // Connect to Snowflake (replace with your credentials)
    var connection = new SnowflakeConnection(
        "account",
        "user",
        "password",
        "database",
        "schema",
        "warehouse"
    );

    // Query column information from info schema
    var columnInfo = connection.ExecuteQuery($"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'");

    // Build TypeScript class with camelCase properties
    var classBuilder = new StringBuilder();
    classBuilder.AppendLine("export class " + ToCamelCase(tableName) + " {");

    foreach (var row in columnInfo)
    {
        string columnName = row["COLUMN_NAME"].ToString();
        string camelCasePropertyName = ToCamelCase(columnName);
        string propertyType = GetColumnTypeInTypeScript(row["DATA_TYPE"].ToString());
        classBuilder.AppendLine($"    public {propertyType} {camelCasePropertyName};");
    }

    classBuilder.AppendLine("}");

    return classBuilder.ToString();
}

// Helper methods
static string ToCamelCase(string text)
{
    if (string.IsNullOrEmpty(text))
    {
        return text;
    }

    char[] chars = text.ToCharArray();
    bool capitalizeNext = true;
    StringBuilder result = new StringBuilder();

    foreach (char c in chars)
    {
        if (char.IsWhiteSpace(c))
        {
            capitalizeNext = true;
        }
        else if (capitalizeNext)
        {
            result.Append(char.ToUpperInvariant(c));
            capitalizeNext = false;
        }
        else
        {
            result.Append(char.ToLowerInvariant(c));
        }
    }

    return result.ToString();
}


static string GetColumnTypeInTypeScript(string snowflakeDataType)
{
    switch (snowflakeDataType.ToUpperInvariant())
    {
        case "NUMBER":
            return "number";
        case "FLOAT":
            return "number";
        case "VARCHAR":
        case "STRING":
            return "string";
        case "DATE":
            return "Date";
        case "TIMESTAMP":
            return "Date";
        case "BOOLEAN":
            return "boolean";
        // Add more cases as needed for other Snowflake data types
        default:
            return "any"; // Use "any" as a fallback for unknown types
    }
}
