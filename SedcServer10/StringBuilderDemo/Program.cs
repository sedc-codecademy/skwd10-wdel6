using System.Diagnostics;
using System.Text;

var bytes = Enumerable.Range(1, 1_000_000);

//Stopwatch sw = Stopwatch.StartNew();
//string result = "";
//foreach (var dataByte in bytes)
//{
//    result += dataByte.ToString() + ":";
//}
//sw.Stop();
//Console.WriteLine(result.Length);
//Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds}ms");


Stopwatch sw2 = Stopwatch.StartNew();
StringBuilder sb = new StringBuilder();
foreach (var dataByte in bytes)
{
    sb.Append(dataByte.ToString() + ":");
}
sw2.Stop();
var result2 = sb.ToString();
Console.WriteLine(result2.Length);
Console.WriteLine($"Elapsed {sw2.ElapsedMilliseconds}ms");


