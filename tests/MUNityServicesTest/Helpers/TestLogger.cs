using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MUNity.Services.Test.Helpers;

public class TestLogger<T> : ILogger<T>
{
    public List<string> Trace { get; set; } = new();

    public List<string> Debug { get; set; } = new();

    public List<string> Information { get; set; } = new();

    public List<string> Warning { get; set; } = new();

    public List<string> Error { get; set; } = new();

    public List<string> Critical { get; set; } = new();

    public IDisposable BeginScope<TState>(TState state)
    {
        // Nothing to see here
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                Trace.Add(state.ToString());
                break;
            case LogLevel.Debug:
                Debug.Add(state.ToString());
                break;
            case LogLevel.Information:
                Information.Add(state.ToString());
                break;
            case LogLevel.Warning:
                Warning.Add(state.ToString());
                break;
            case LogLevel.Error:
                Error.Add(state.ToString());
                break;
            case LogLevel.Critical:
                Critical.Add(state.ToString());
                break;
            case LogLevel.None:
                break;
            default:
                break;
        }
    }
}
