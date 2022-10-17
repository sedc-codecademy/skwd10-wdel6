using Sedc.Server.Attributes;

internal class Operation
{
    public string Symbol { get; set; }
    public string Name { get; set; }

    public static Operation Subtract = new Operation { Name = "Subtraction", Symbol = "-" };

    public static Operation Divide = new Operation { Name = "Division", Symbol = "/" };
}

internal class Model
{
    public int First { get; set; }
    public int Second { get; set; }
    public Operation Operation { get; set; }
    public int Result { get; set; }
    public bool Success { get; set; }

    public override string ToString()
    {
        return "She's a model and she's looking good";
    }
}

internal class Calculator
{
    public Model Add(int first, int second)
    {
        return new Model { 
            First = first, 
            Second = second,
            Operation = new Operation { Name = "Addition", Symbol = "+"},
            Result = first + second,
            Success = true
        };
    }

    [RouteName("Sub")]
    public Model Subtract(int first, int second)
    {
        return new Model
        {
            First = first,
            Second = second,
            Operation = Operation.Subtract,
            Result = first - second,
            Success = true
        };
    }

    [RouteName("Mul")]
    public Model Multiply(int first, int second)
    {
        return new Model
        {
            First = first,
            Second = second,
            Operation = new Operation { Name = "Multiplication", Symbol = "*" },
            Result = first * second,
            Success = true
        };
    }

    [RouteName("Div")]
    public Model Divide(int first, int second)
    {
        if (second == 0)
        {
            return new Model
            {
                First = first,
                Second = second,
                Operation = Operation.Divide,
                Success = false
            };
        }
        return new Model
        {
            First = first,
            Second = second,
            Operation = Operation.Divide,
            Result = first / second,
            Success = true
        };
    }

}