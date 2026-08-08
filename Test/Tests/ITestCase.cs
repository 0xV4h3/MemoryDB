using Test.Infrastructure;

namespace Test.Tests;

public interface ITestCase
{
    string Name { get; }
    Result<TestExecutionReport> Run(TestConfig config);
}