using System.Reflection;
using Moq;

namespace MockExtension;

public class MockExtensionBuilder
{
    private static Type voidType = typeof(void);
    
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

        var parameters = method.GetParameters().Select(CreateParameter).ToList();
        if (returnType.Name is not "Void") {
            
            parameters.Add(new Parameter(isAsync ? taskType! : returnType, "returnValue"));
        }
        parameters.Add(new Parameter(typeof(Moq.Times), "times"));
        var parameterList = string.Join(", ", parameters);
        
        return returnType switch {
            { Name: "Void" } => BuildMethodWithVoidReturn(method, type, isAsync, parameterList),
            { Name: "Task" } => BuildMethodWithReturn(method, type, isAsync, parameterList),
            { } => BuildMethodWithReturn(method, type, isAsync, parameterList),
            null => throw new Exception("Retyirn type is null")
        };        
    }

    private static string BuildMethodWithReturn(MethodInfo method, Type type, bool isAsync, string parameterList)
    {
        return $@"public static void With{method.Name}(this Mock<{type.Name}> mock, {parameterList})
{{
    mock.Setup(x => x.{method.Name}())
        .Returns{(isAsync ? "Async" : "")}(returnValue)
        .Verifiable(times)
}}";
    }

    private static string BuildMethodWithVoidReturn(MethodInfo method, Type type, bool isAsync, string parameterList)
    {
        return $@"public static void With{method.Name}(this Mock<{type.Name}> mock, {parameterList})
{{
    mock.Setup(x => x.{method.Name}())
        .Verifiable(times)
}}";
    }

    private Parameter CreateParameter(ParameterInfo parameterInfo)
        => new Parameter(parameterInfo.GetType(),
            parameterInfo.Name ?? throw new Exception());
}
