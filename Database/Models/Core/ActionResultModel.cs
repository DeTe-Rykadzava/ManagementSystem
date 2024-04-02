namespace Database.Models.Core;

public class ActionResultModel<T>
{
    public bool IsSuccess { get; internal set; }
    public List<ActionResultType> ResultTypes { get; internal set; } = new List<ActionResultType>();
    public T? Value { get; internal set;}

    internal ActionResultModel(List<ActionResultType> resultTypes, T? value, bool success = false) => 
        (IsSuccess, ResultTypes, Value) = (success, resultTypes, value);

    internal ActionResultModel() { }
}