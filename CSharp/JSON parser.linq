<Query Kind="Program">
  <Reference>C:\Users\M755411\.nuget\packages\Newtonsoft.Json\11.0.2\lib\net45\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	JsonTest2();
}

//To pretty-print an object:
//JToken.FromObject(myObject).ToString()

//Prettify existing JSON: (edit: using JSON.net)
//JToken.Parse("mystring").ToString()


/*
{"name": "John Smith", "Sport": "Wrestling"}

*/
public class Cleanseable<T>
{
    public T Id { get; set; }

    public string Value { get; set; }

    public bool IsCleansed { get; set; }

    public Cleanseable()
    {

    }

    public Cleanseable(T id, string value, bool isCleansed = false)
    {
        Id = id;
        Value = value;
        IsCleansed = isCleansed;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Value: {Value}, IsCleansed = {IsCleansed}";
    }
}


void JsonTest1()
{
    var jsonData=Util.ReadLine<string>("Enter JSON string:");
    var jsonAsObject = JsonConvert.DeserializeObject<object>(jsonData);
    jsonAsObject.Dump("Deserialized JSON");
}

void JsonTest2()
{
	var data = Enumerable.Range(1, 3)
		.Select(s => new Cleanseable<int>(s, "Value "+ s.ToString()));
    
	var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
    json.Dump("JSON");

    var jsonAsObject = JsonConvert.DeserializeObject<Cleanseable<int>[]>(json);
	jsonAsObject.Dump();
}

void JsonTest3()
{

}