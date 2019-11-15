<Query Kind="Program">
  <Reference>C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\FastMember.1.1.0\lib\net40\FastMember.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

async Task  Main()
{
            const int MaxClients = 12;

            var tasks = new List<Task<int>>();
            var clientIds = Enumerable.Range(1001, MaxClients);

            try
            {
                const int MaxTasks = 2;
                var semaphoreSlim = new SemaphoreSlim(MaxTasks);

                foreach (var id in clientIds)
                {
                    //Console.WriteLine($"About to process client {id} at {DateTime.Now}");
                    await semaphoreSlim.WaitAsync().ConfigureAwait(false);
                    Console.WriteLine($"Processing client {id} at {DateTime.Now}");
					var task = DummyGetTestNumber(id);
                    var cont = task.ContinueWith(t => semaphoreSlim.Release());
					tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("AggregateException: " + ex.InnerExceptions[0].Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {			
                Console.WriteLine("Complete\r\n");
                foreach (var t in tasks)
                {
                    Console.WriteLine(
                        $"Task {t.Id} status: {t.Status}, IsCompleted: {t.IsCompleted}, Return value: {t.Result}");
                }

            }
	
}

// Define other methods and classes here
private async Task <int> DummyGetTestNumber(int clientId)
{
    await Task.Delay(1500);
    //var rand = new Random();
    //Task.Delay(rand.Next(50, 500));

    if (clientId == 1001)
    {
	    await Task.Delay(3000);
    }
	
    if (clientId == 1007)
    {
        //throw new ArgumentException("Can't process clientId 1007");
    }
	
    return await Task.FromResult(clientId);
}