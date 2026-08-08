namespace Test.Infrastructure;

public sealed class TestConfig
{
    public string RootPath { get; init; } = "";
    private static string ResultPath { get; } = "TestResults";
    public bool SaveArtifacts { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public string TimestampFormat { get; init; } = "yyyyMMdd_HHmmss";
    public static TestConfig CreateDefault()
    {
        string exeDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Directory.GetParent(exeDir)?.Parent?.Parent?.Parent?.FullName
                            ?? throw new InvalidOperationException("Project directory was not resolved.");
        string root = Path.Combine(projectDir, ResultPath);
        Directory.CreateDirectory(root);
        return new TestConfig { RootPath = root };
    }
}