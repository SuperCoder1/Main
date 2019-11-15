<Query Kind="Program">
  <Reference>C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\FastMember.1.1.0\lib\net40\FastMember.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async void Main()
{
    var stopWatch = Stopwatch.StartNew();
    await Task.Delay(62000);
    stopWatch.Stop();
	stopWatch.Dump();
	(stopWatch.ElapsedMilliseconds/1000).Dump();
}

// Define other methods and classes here
