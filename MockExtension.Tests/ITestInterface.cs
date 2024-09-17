namespace MockExtension.Tests;

interface ITestInterface 
{
	string NoArgs();	

	Task<string> NoArgsAsync();
}
