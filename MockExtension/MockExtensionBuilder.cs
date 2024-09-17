using System.Reflection;
using Moq;

namespace MockExtension;

public class MockExtensionBuilder
{
    public IEnumerable<string> BuildMockExtensions<T>()
        where T : class
    {
        Type type = ExtractType<T>();

        foreach (var method in type.GetMethods())
        {
            string methodText = GenerateMethod<T>(method);
            yield return methodText;
        }
    }

    private static Type ExtractType<T>() where T : class
    {
        Type type = typeof(T);
        var targetInterface = type.IsInterface
            ? type
            : type.GetInterface($"I{type.Name}")
                ?? throw new Exception("Could not find elegible interface for type");
        return targetInterface;
    }

    public string GenerateMethod<T>(MethodInfo method)
        where T : class
    {
        var type = ExtractType<T>();
        var returnType = method.ReturnType;
        var isAsync = false;
        var taskType = returnType.GenericTypeArguments.FirstOrDefault();
        if (taskType != null)
        {
            isAsync = true;
            returnType = taskType;
        }

        var parameters = method.GetParameters();
        var parameterList = string.Join(", ", parameters.Select(CreateParameter)
            .Append(new Parameter(isAsync ? taskType! : returnType, "returnType"))
            .Append(new Parameter(typeof(Moq.Times), "times")));
        var methodText =
$@"public static void With{method.Name}(this Mock<{type.Name}> mock, {parameterList})
{{
    mock.Setup(x => x.{method.Name}())
        .Returns{(isAsync ? "Async" : "")}(returnValue)
        .Verifiable(times)
}}";
        return methodText;
    }

    private Parameter CreateParameter(ParameterInfo parameterInfo)
        => new Parameter(parameterInfo.GetType(),
            parameterInfo.Name ?? throw new Exception());
}
