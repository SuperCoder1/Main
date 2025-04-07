using Azure.ResourceManager.DataFactory;
using Azure.ResourceManager.DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Assuming adfResource is already initialized
            CopyActivitiesWithDependency(adfResource);
            Console.WriteLine("Pipeline activities copied and dependency configured successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CopyActivitiesWithDependency(DataFactoryResource adfResource)
    {
        // Define pipeline names
        string sourcePipelineName = "sourceTemplate";
        string targetPipelineName = "targetTemplate";
        string dependencyActivityName = "t_dependency";

        // Get source pipeline
        DataFactoryPipelineResource sourcePipeline = adfResource.GetDataFactoryPipeline(sourcePipelineName).Value;
        DataFactoryPipelineResource targetPipeline = adfResource.GetDataFactoryPipeline(targetPipelineName).Value;

        // Get activities from source pipeline
        var sourceActivities = sourcePipeline.Data.PipelineActivities.ToList();

        // Create new list for target pipeline activities
        var targetActivities = targetPipeline.Data.PipelineActivities != null 
            ? targetPipeline.Data.PipelineActivities.ToList() 
            : new List<DataFactoryActivity>();

        // Copy all activities from source to target
        foreach (var activity in sourceActivities)
        {
            // Create a deep copy of the activity
            var newActivity = CloneActivity(activity);
            targetActivities.Add(newActivity);
        }

        // Find the dependency activity in target activities
        var dependencyActivity = targetActivities.FirstOrDefault(a => a.Name == dependencyActivityName);
        
        if (dependencyActivity == null)
        {
            throw new Exception($"Dependency activity '{dependencyActivityName}' not found in source template.");
        }

        // Add dependency to first activity if there are activities
        if (targetActivities.Any() && targetActivities[0].Name != dependencyActivityName)
        {
            var firstActivity = targetActivities[0];
            
            // Initialize dependsOn collection if null
            firstActivity.DependsOn = firstActivity.DependsOn ?? new List<ActivityDependency>();
            
            // Add success dependency
            firstActivity.DependsOn.Add(new ActivityDependency
            {
                ActivityName = dependencyActivityName,
                DependencyConditions = new List<string> { "Succeeded" }
            });
        }

        // Update target pipeline with new activities
        targetPipeline.Data.PipelineActivities = targetActivities;
        
        // Save the changes
        targetPipeline.Update().WaitForCompletionAsync().ConfigureAwait(false);
    }

    static DataFactoryActivity CloneActivity(DataFactoryActivity source)
    {
        // This is a basic implementation - you might need to extend it based on specific activity types
        return new DataFactoryActivity(source.Name)
        {
            Description = source.Description,
            DependsOn = source.DependsOn != null 
                ? new List<ActivityDependency>(source.DependsOn) 
                : null,
            UserProperties = source.UserProperties != null 
                ? new List<DataFactoryUserProperty>(source.UserProperties) 
                : null,
            Type = source.Type,
            // Add other properties as needed based on your specific activity types
        };
    }
}using Azure.ResourceManager.DataFactory;
using Azure.ResourceManager.DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Assuming adfResource is already initialized
            CopyActivitiesWithDependency(adfResource);
            Console.WriteLine("Pipeline activities copied and dependency configured successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CopyActivitiesWithDependency(DataFactoryResource adfResource)
    {
        // Define pipeline names
        string sourcePipelineName = "sourceTemplate";
        string targetPipelineName = "targetTemplate";
        string dependencyActivityName = "t_dependency";

        // Get source pipeline
        DataFactoryPipelineResource sourcePipeline = adfResource.GetDataFactoryPipeline(sourcePipelineName).Value;
        DataFactoryPipelineResource targetPipeline = adfResource.GetDataFactoryPipeline(targetPipelineName).Value;

        // Get activities from source pipeline
        var sourceActivities = sourcePipeline.Data.PipelineActivities.ToList();

        // Create new list for target pipeline activities
        var targetActivities = targetPipeline.Data.PipelineActivities != null 
            ? targetPipeline.Data.PipelineActivities.ToList() 
            : new List<DataFactoryActivity>();

        // Copy all activities from source to target
        foreach (var activity in sourceActivities)
        {
            // Create a deep copy of the activity
            var newActivity = CloneActivity(activity);
            targetActivities.Add(newActivity);
        }

        // Find the dependency activity in target activities
        var dependencyActivity = targetActivities.FirstOrDefault(a => a.Name == dependencyActivityName);
        
        if (dependencyActivity == null)
        {
            throw new Exception($"Dependency activity '{dependencyActivityName}' not found in source template.");
        }

        // Add dependency to first activity if there are activities
        if (targetActivities.Any() && targetActivities[0].Name != dependencyActivityName)
        {
            var firstActivity = targetActivities[0];
            
            // Initialize dependsOn collection if null
            firstActivity.DependsOn = firstActivity.DependsOn ?? new List<ActivityDependency>();
            
            // Add success dependency
            firstActivity.DependsOn.Add(new ActivityDependency
            {
                ActivityName = dependencyActivityName,
                DependencyConditions = new List<string> { "Succeeded" }
            });
        }

        // Update target pipeline with new activities
        targetPipeline.Data.PipelineActivities = targetActivities;
        
        // Save the changes
        targetPipeline.Update().WaitForCompletionAsync().ConfigureAwait(false);
    }

    static DataFactoryActivity CloneActivity(DataFactoryActivity source)
    {
        // This is a basic implementation - you might need to extend it based on specific activity types
        return new DataFactoryActivity(source.Name)
        {
            Description = source.Description,
            DependsOn = source.DependsOn != null 
                ? new List<ActivityDependency>(source.DependsOn) 
                : null,
            UserProperties = source.UserProperties != null 
                ? new List<DataFactoryUserProperty>(source.UserProperties) 
                : null,
            Type = source.Type,
            // Add other properties as needed based on your specific activity types
        };
    }
}
