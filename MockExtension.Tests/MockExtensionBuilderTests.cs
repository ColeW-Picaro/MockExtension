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
@"public static void WithNoArgs(this Mock<ITestInterface> mock, string returnValue, Moq.Times times)
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
@"public static void WithNoArgsAsync(this Mock<ITestInterface> mock, string returnValue, Moq.Times times)
{
    mock.Setup(x => x.NoArgsAsync())
        .ReturnsAsync(returnValue)
        .Verifiable(times)
}");
    }

    [Fact]
    public void NoArgsNoReturnAsync_GeneratesExtension() {
        var method = typeof(ITestInterface).GetMethod("NoArgsNoReturn") ?? throw new Exception();
        Assert.Equal(
        _mockExtensionBuilder.GenerateMethod<ITestInterface>(method), 
@"public static void WithNoArgsNoReturn(this Mock<ITestInterface> mock, Moq.Times times)
{
    mock.Setup(x => x.NoArgsNoReturn())
        .Verifiable(times)
}");
    }
    
    [Fact]
    public void ValueTypeArgs_GeneratesExtension() {
        var method = typeof(ITestInterface).GetMethod("ValueTypeArgs") ?? throw new Exception();
        Assert.Equal(
        _mockExtensionBuilder.GenerateMethod<ITestInterface>(method), 
@"public static void WithValueTypeArgs(this Mock<ITestInterface> mock, int a, float b, double c, bool d, char e, ValueTypeStruct f, ValueTypeEnum g, Moq.Times times)
{
    mock.Setup(x => x.ValueTypeArgs(a, b, c, d, e, f, g))
        .Verifiable(times)
}");
    }
}
