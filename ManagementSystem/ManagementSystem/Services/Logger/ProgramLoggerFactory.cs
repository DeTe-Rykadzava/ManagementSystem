using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.Logger;

public class ProgramLoggerFactory : IProgramLoggerFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public ProgramLoggerFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public ILogger<T> CreateLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerFactory.CreateLogger(categoryName);
    }
}