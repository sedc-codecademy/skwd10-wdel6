<Query Kind="Program" />

void Main()
{
	
}

// You can define other methods, fields, classes and namespaces here

interface IAuthProvider 
{
	string PasswordRequirements {get; set;}
	string UserNameFormat {get; set;}
	int LoginAttempts {get; set;}
	bool UseMFA {get; set;} = false;
}
