namespace MockExtension;

public class Parameter(Type type, string name) {

	public override string ToString() {
		return $"{GetTypeString()} {name}";
	}

	private string GetTypeString() {
		return $"{type}" switch {
			"System.String" => "string",
			"System.SByte" => "sbyte",
			"System.Byte" => "byte",
			"System.Int16" => "short",
			"System.UInt16" => "ushort",
			"System.Int32" => "int",
			"System.UInt32" => "uint",
			"System.Int64" => "long",
			"System.UInt64" => "ulong",
			"System.Char" => "char",
			"System.Single" => "float",
			"System.Double" => "double",
			"System.Boolean" => "bool",
			"System.Decimal" => "decimal",
			_ => type.ToString()
		};
	}
}
