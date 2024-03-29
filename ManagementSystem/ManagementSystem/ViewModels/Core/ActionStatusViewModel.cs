using System.Collections.Generic;

namespace ManagementSystem.ViewModels.Core;

public class ActionStatusViewModel<T>
{
    public bool Success { get; }
    public List<string> Errors { get; }
    public T? Value { get; }

    public ActionStatusViewModel(List<string> errors, T? value, bool success = false)
    {
        Success = success;
        Errors = errors;
        Value = value;
    }
}