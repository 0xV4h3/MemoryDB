using Test.Tests;

namespace Test.Infrastructure;

public sealed class TestRunner(TestConfig config)
{
    private readonly TestConfig _config = config;
    private readonly List<ITestCase> _tests = [];

    public TestRunner Add(ITestCase test)
    {
        _tests.Add(test);
        return this;
    }

    public IReadOnlyList<Result<TestExecutionReport>> RunAll()
    {
        List<Result<TestExecutionReport>> results = [];
        foreach (var test in _tests)
        {
            var result = test.Run(_config);
            results.Add(result);
            if (_config.FailFast && !result.IsSuccess) break;
        }
        return results;
    }
}