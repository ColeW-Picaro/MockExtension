namespace MockExtension.Tests;

struct ValueTypeStruct(int x, int y);

enum ValueTypeEnum {
	Red,
	Blue,
	Green	
}

interface ITestInterface 
{
	string NoArgs();	

	Task<string> NoArgsAsync();

	void NoArgsNoReturn();

	void ValueTypeArgs(int a, float b, double c, bool d, char e, ValueTypeStruct f, ValueTypeEnum g);
}
