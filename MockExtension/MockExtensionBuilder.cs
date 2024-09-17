using System.Reflection;
using Moq;

namespace MockExtension;

public class MockExtensionBuilder
{
    public IEnumerable<string> BuildMockExtensions<T>()
        where T: class
    {
        Type type = typeof(T);
        var targetInterface = type.IsInterface
            ? type
            : type.GetInterface($"I{type.Name}")
                ?? throw new Exception("Could not find elegible interface for type");

        foreach (var method in targetInterface.GetMethods())
        {
            var returnType = method.ReturnType;
            var taskType = returnType.GenericTypeArguments.FirstOrDefault();
            if (taskType != null) {
                returnType = taskType;
            }
                
            var thisParameter = new Parameter(typeof(Mock<T>), "mock");
            var parameters = method.GetParameters();
            string parameterList = string.Join(", ", parameters.Select(CreateParameter)
                .Prepend(thisParameter)
                .Append(new Parameter(returnType, "returnType"))
                .Append(new Parameter(typeof(Moq.Times), "times")));
            var methodText = 
$@"public static void With{method.Name}({parameterList})
{{
    mock.Setup(x => x.{method.Name}())
        .Returns{(taskType != null ? "Async" : "")}(returnValue)
        .Verifiable(times)
}}";
            yield return methodText;
        }
    }

    private Parameter CreateParameter(ParameterInfo parameterInfo) => new Parameter(parameterInfo.GetType(), parameterInfo.Name);
}
