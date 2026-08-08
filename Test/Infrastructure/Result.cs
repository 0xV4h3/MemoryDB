namespace Test.Infrastructure;

public sealed class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string Error { get; init; } = string.Empty;
    public List<string> Logs { get; init; } = [];

    public static Result<T> Success(T value, List<string>? logs = null) =>
        new() { IsSuccess = true, Value = value, Logs = logs ?? [] };

    public static Result<T> Failure(string error, List<string>? logs = null, T? value = default) =>
        new() { IsSuccess = false, Error = error, Logs = logs ?? [], Value = value };
}