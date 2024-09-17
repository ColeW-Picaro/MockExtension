namespace MockExtension.Tests;

public class MockExtensionBuilderTests
{
    private readonly MockExtensionBuilder _mockExtensionBuilder;

    public MockExtensionBuilderTests()
    {
        _mockExtensionBuilder = new();
    }

    [Fact]
    public void Test1()
    {

        var methods = _mockExtensionBuilder.BuildMockExtensions<ITestInterface>();
        foreach (var method in methods)
        {
            Console.WriteLine(method);
        }

    }
}
