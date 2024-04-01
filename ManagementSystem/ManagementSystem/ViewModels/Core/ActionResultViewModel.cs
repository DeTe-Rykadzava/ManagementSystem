using System.Collections.Generic;
using Database.Models.Core;

namespace ManagementSystem.ViewModels.Core;

public class ActionResultViewModel<T>
{
    public bool IsSuccess { get; internal set; }
    public List<string> Statuses { get; } = new List<string>();
    public T? Value { get; internal set; }

    internal ActionResultViewModel(List<string> statuses, T? value, bool isSuccess = false)
    {
        IsSuccess = isSuccess;
        Statuses = statuses;
        Value = value;
    }
    internal ActionResultViewModel() { }
}