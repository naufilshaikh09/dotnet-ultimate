namespace dotnet_ultimate.Services;

public class DummyService(ILogger<DummyService> logger) : IDummyService
{
    public void DoSomething()
    {
        logger.LogInformation("Doing something - info");
        logger.LogCritical("Doing something - critical");
        logger.LogDebug("Doing something - debug");
        logger.LogInformation("Invoking {@Event} with ID as {@Id}", "SomeEvent", Guid.NewGuid());
    }
}