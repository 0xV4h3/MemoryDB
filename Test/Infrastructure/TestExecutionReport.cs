namespace Test.Infrastructure;

public sealed record TestExecutionReport(
    string TestName,
    DateTime StartedUtc,
    DateTime FinishedUtc,
    bool Success,
    string Error,
    IReadOnlyList<string> Logs
);