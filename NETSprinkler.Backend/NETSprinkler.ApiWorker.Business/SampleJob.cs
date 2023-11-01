using Hangfire.Console.Extensions;
using Microsoft.Extensions.Logging;

namespace NETSprinkler.ApiWorker.Business;

public class SampleJob
{
    private readonly ILogger<SampleJob> logger;
    //private readonly IProgressBarFactory progressBarFactory;
    private readonly IJobManager jobManager;

    public SampleJob(ILogger<SampleJob> logger, /*IProgressBarFactory progressBarFactory,*/ IJobManager jobManager)
    {
        this.logger = logger;
       // this.progressBarFactory = progressBarFactory;
        this.jobManager = jobManager;
    }

    public Task RunAsync(int id)
    {
        logger.LogTrace("Test {Id}", id);
        logger.LogDebug("Test {Id}", id);
        logger.LogInformation("Test {Id}", id);
        logger.LogWarning("Test {Id}", id);
        logger.LogError("Test {Id}", id);
        logger.LogCritical("Test {Id}", id);
        return Task.CompletedTask;

       /* var progress = progressBarFactory.Create("Test");
        for (var i = 0; i < 100; i++)
        {
            progress.SetValue(i + 1);
            await Task.Delay(100);
        }*/

        //Starting a job inside a job will mark it as a Continuation
       // jobManager.Start<ContinuationJob>(x => x.RunAsync());
    }
}