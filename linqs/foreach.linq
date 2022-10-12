<Query Kind="Program" />

void Main()
{
	var things = new Things();
	//things.Where(x => x > 1).Dump();
	
	foreach (var thing in things) {
		thing.Dump();
	}
	
}

// You can define other methods, fields, classes and namespaces here
    internal class Things
    {
        public IEnumerator GetEnumerator()
        {
            yield return 1;
            yield return 5;
            yield return 3;
			yield break;
        }
    }