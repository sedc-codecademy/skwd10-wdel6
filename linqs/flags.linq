<Query Kind="Program" />

[Flags]
enum Pizza {
	None = 0,
	Mozarella = 1,
	Mushroom = 2,
	Ham = 4,
	Bacon = 8,
	
	Vezuvio = 5,
	Capriciossa = 7
}

void Main()
{
	int i = (int) ( Pizza.Vezuvio | Pizza.Bacon);
	i.Dump();
	Pizza e = (Pizza) i;
	e.Dump();
}

// You can define other methods, fields, classes and namespaces here