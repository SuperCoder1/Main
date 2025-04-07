using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.DataFactory;
using Azure.ResourceManager.DataFactory.Models;

namespace CopyDataFactoryActivities
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Replace with your actual resource group name and data factory name
            string resourceGroupName = "your-resource-group-name";
            string dataFactoryName = "your-data-factory-name";
            string sourceTemplateName = "sourceTemplate";
            string targetTemplateName = "targetTemplate";
            string dependencyActivityName = "t_dependency";

            try
            {
                // Authenticate using DefaultAzureCredential (or other appropriate method)
                var credential = new DefaultAzureCredential();
                var client = new ArmClient(credential);

                // Get the DataFactoryResource
                ResourceIdentifier dataFactoryResourceId = DataFactoryResource.CreateResourceIdentifier(
                    subscriptionId: (await client.GetSubscriptionAsync()).Value.Id.Name,
                    resourceGroupName: resourceGroupName,
                    dataFactoryName: dataFactoryName);
                DataFactoryResource adfResource = await client.GetDataFactoryResource(dataFactoryResourceId).GetAsync();

                // Get the source and target pipeline templates
                PipelineResource sourceTemplate = await adfResource.GetPipelineAsync(sourceTemplateName);
                PipelineResource targetTemplate = await adfResource.GetPipelineAsync(targetTemplateName);

                if (sourceTemplate == null || targetTemplate == null)
                {
                    Console.WriteLine("Source or target pipeline template not found.");
                    return;
                }

                // Clone activities from the source template to the target template
                var newActivities = new Dictionary<string, PipelineActivity>();
                foreach (var activityPair in sourceTemplate.Data.Activities)
                {
                    var newActivity = activityPair.Value.Clone();
                    newActivities.Add(activityPair.Key, newActivity);
                }

                // Find the dependency activity in the source template
                if (!sourceTemplate.Data.Activities.TryGetValue(dependencyActivityName, out var dependencyActivity))
                {
                    Console.WriteLine($"Dependency activity '{dependencyActivityName}' not found in the source template.");
                    return;
                }

                // Get the first activity in the target template
                if (targetTemplate.Data.Activities.Count == 0)
                {
                    Console.WriteLine("Target template has no activities to create a precedent constraint with.");
                    return;
                }
                var firstTargetActivity = targetTemplate.Data.Activities.First().Value;

                // Create a success precedent constraint
                foreach (var activityPair in newActivities)
                {
                    if (activityPair.Key == dependencyActivityName)
                    {
                        continue; // Don't add a dependency on itself
                    }

                    if (activityPair.Value.DependsOn == null)
                    {
                        activityPair.Value.DependsOn = new List<ActivityDependency>();
                    }
                    activityPair.Value.DependsOn.Add(new ActivityDependency()
                    {
                        Activity = firstTargetActivity.Name,
                        DependencyCondition = new List<string>() { "Succeeded" }
                    });
                }

                // Update the target template with the new activities and dependency
                PipelinePatch targetTemplatePatch = new PipelinePatch()
                {
                    Activities = targetTemplate.Data.Activities.Concat(newActivities).ToDictionary(pair => pair.Key, pair => pair.Value)
                };

                // Update the target pipeline template
                ArmOperation<PipelineResource> updateOperation = await adfResource.GetPipeline(targetTemplateName).UpdateAsync(WaitUntil.Completed, targetTemplatePatch);
                PipelineResource updatedTargetTemplate = updateOperation.Value;

                Console.WriteLine($"Successfully copied activities from '{sourceTemplateName}' to '{targetTemplateName}' and created a success precedent constraint.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
