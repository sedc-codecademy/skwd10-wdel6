<Query Kind="Program">
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

void Main()
{
	var list = new List<int>{1, 2, 3, 4, 5, 6};
	
	list.Select(x => {
		x.Dump();
		return x*x;
	}).First(x => x > 10);
	
	var someEvens = GetAllEvenNumbers().TakeWhile(x => x<10);

	"BEFORE".Dump();
	foreach(var even in someEvens) {};
	"HERE".Dump();
	
}

// You can define other methods, fields, classes and namespaces here

IEnumerable<int> GetAllEvenNumbers() {
	var value = 0;
	while(true) {
		$"Returning {value}".Dump();
		yield return value;
		value +=2;
	}
}


