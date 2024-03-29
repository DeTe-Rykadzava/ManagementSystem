using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.Logger;

public interface IProgramLoggerFactory
{
    public ILogger<T> CreateLogger<T>();
    public ILogger CreateLogger(string categoryName);
}