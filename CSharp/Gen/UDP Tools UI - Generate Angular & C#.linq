<Query Kind="Program" />

void Main()
{
    var createTableSql = @"CREATE OR REPLACE TABLE OrderSummary (
  CUST_KEY number default NULL,
  ORDER_DATE date default NULL,
  ORDER_STATUS varchar(100) default NULL,
  PRICE number
)";

    var tableName = Regex.Match(createTableSql, @"CREATE OR REPLACE TABLE (\w+)").Groups[1].Value;
    var columnMatches = Regex.Matches(createTableSql, @"(\w+) (number|date|varchar\(\d+\))");
    var columns = columnMatches.Cast<Match>()
        .Select(m => new Column
        {
            Name = m.Groups[1].Value,
            Type = m.Groups[2].Value == "number" ? "number" : "string",
            JsonType = m.Groups[2].Value == "number" ? "int" : "string"
        })
        .ToArray();

    // Generate TypeScript class
    var tsClass = $@"export class {tableName} {{
{string.Join("\n", columns.Select(c => $"  {c.Name}: {c.Type};"))}
}}";
    tsClass.Dump("TypeScript Class");

// Generate C# class with JsonProperty annotations
var csharpClass = $@"public class {tableName}
{{
{string.Join("\n", columns.Select(c => $"  [JsonProperty(\"{c.Name}\")]\n  public {c.JsonType} {c.Name} {{ get; set; }}"))}
}}";
    csharpClass.Dump("C# Class");

    // Generate Angular component
    var angularComponent = $@"import {{ Component }} from '@angular/core';
import {{ {tableName} }} from './{tableName.ToLower()}';

@Component({{
  selector: 'app-{tableName.ToLower()}',
  template: `
<kendo-grid [data]='gridData'>
{string.Join("\n", columns.Select(c => $"  <kendo-grid-column field='{c.Name}' title='{c.Name}'></kendo-grid-column>"))}
</kendo-grid>
`
}})
export class {tableName}Component {{
  public gridData: {tableName}[] = [];
}}";
    angularComponent.Dump("Angular Component");

// Generate Angular component
//var angularComponent2 = $@"import {{ Component }} from '@angular/core';
//import {{ {tableName} }} from './{tableName.ToLower()}';
//
//@Component({{
//  selector: 'app-{tableName.ToLower()}',
//  template: `
//<kendo-grid [data]='gridData'>
//  <kendo-grid-column field='{columns[0].Name}' title='{columns[0].Name}'></kendo-grid-column>
//  <kendo-grid-column field='{columns[1].Name}' title='{columns[1].Name}'></kendo-grid-column>
//  <kendo-grid-column field='{columns[2].Name}' title='{columns[2].Name}'></kendo-grid-column>
//  <kendo-grid-column field='{columns[3].Name}' title='{columns[3].Name}'></kendo-grid-column>
//</kendo-grid>
//`
//}})
//export class {tableName}Component {{
//  public gridData: {tableName}[] = [];
//}}";
//    angularComponent2.Dump("Angular Component");
}

class Column
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string JsonType { get; set; }
}