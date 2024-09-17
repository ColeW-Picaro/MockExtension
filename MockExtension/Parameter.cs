namespace MockExtension;

public class Parameter(Type Type, string Name) {
	public override string ToString() {
		return $"{Type} {Name}";
	}
}
