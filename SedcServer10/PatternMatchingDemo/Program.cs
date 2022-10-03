// See https://aka.ms/new-console-template for more information
using PatternMatchingDemo;


var weko = new Person
{
    FirstName = "Wekoslav",
    LastName = "Stefanovski"
};

var bill = new Person
{
    FirstName = "Bil",
    LastName = "Gates"
};

var brad = new Person
{
    FirstName = "Brad",
    LastName = "Pitt"
};

var Greet = (Person p) => p switch
{
    { FirstName: "Wekoslav"} => "Hi, Weko",
    { LastName: "Gates" } => "Hi, Mr. Gates",
    _ => "Hi, stranger"
};

Console.WriteLine(Greet(weko));
Console.WriteLine(Greet(bill));
Console.WriteLine(Greet(brad));

////

var values = (1, "Wekoslav", 3.14, DateTime.Now);

var (_, _, pie, _) = values;

var pie2 = values.Item3;

var Greet2 = ((int a, string b, double c, DateTime d) value) => value switch
{
    (_, _, 3.14, _) => "It's a PIE",
    _ => "It's not a pie :("
};



