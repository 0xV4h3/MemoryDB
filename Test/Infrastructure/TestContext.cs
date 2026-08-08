namespace Test.Infrastructure;

public sealed class TestContext
{
    public string TestName { get; }
    public string TestRunPath { get; }
    public TestConfig Config { get; }

    public TestContext(TestConfig config, string testName)
    {
        Config = config;
        TestName = testName;
        string runFolder = DateTime.UtcNow.ToString(config.TimestampFormat);
        TestRunPath = Path.Combine(config.RootPath, testName, runFolder);
        if (config.SaveArtifacts) Directory.CreateDirectory(TestRunPath);
    }

    public string ArtifactPath(string fileName)
    {
        if (!Config.SaveArtifacts) throw new InvalidOperationException("Artifacts are disabled.");
        return Path.Combine(TestRunPath, fileName);
    }
}