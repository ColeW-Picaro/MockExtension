namespace MockExtension.Tests;

public class MockExtensionBuilderTests
{
    private readonly MockExtensionBuilder _mockExtensionBuilder;

    public MockExtensionBuilderTests()
    {
        _mockExtensionBuilder = new();
    }

    [Fact]
    public void NoArgs()
    {
        var methods = _mockExtensionBuilder.BuildMockExtensions<ITestInterface>();
        foreach (var method in methods)
        {
            Console.WriteLine(method);
        }
    }

    [Fact]
    public void NoArgs_GeneratesExtension() {
        var method = typeof(ITestInterface).GetMethod("NoArgs") ?? throw new Exception();
        Assert.Equal(
        _mockExtensionBuilder.GenerateMethod<ITestInterface>(method), 
@"public static void WithNoArgs(this Mock<ITestInterface> mock, System.String returnType, Moq.Times times)
{
    mock.Setup(x => x.NoArgs())
        .Returns(returnValue)
        .Verifiable(times)
}");
    }
    
    [Fact]
    public void NoArgsAsync_GeneratesExtension() {
        var method = typeof(ITestInterface).GetMethod("NoArgsAsync") ?? throw new Exception();
        Assert.Equal(
        _mockExtensionBuilder.GenerateMethod<ITestInterface>(method), 
@"public static void WithNoArgsAsync(this Mock<ITestInterface> mock, System.String returnType, Moq.Times times)
{
    mock.Setup(x => x.NoArgsAsync())
        .ReturnsAsync(returnValue)
        .Verifiable(times)
}");
    }
}
