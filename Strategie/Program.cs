using System;

// Strategy Interface
public interface IStrategy
{
    void Execute();
}

// Concrete Strategy A
public class ConcreteStrategyA : IStrategy
{
    public void Execute()
    {
        Console.WriteLine("Strategy A: Executed");
    }
}

// Concrete Strategy B
public class ConcreteStrategyB : IStrategy
{
    public void Execute()
    {
        Console.WriteLine("Strategy B: Executed");
    }
}

// Concrete Strategy C
public class ConcreteStrategyC : IStrategy
{
    public void Execute()
    {
        Console.WriteLine("Strategy C: Executed");
    }
}

// Context
public class Context
{
    private IStrategy _strategy;

    public Context(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecuteStrategy()
    {
        _strategy.Execute();
    }
}

// Client Code
public class Program
{
    public static void Main(string[] args)
    {
        Context context;

        context = new Context(new ConcreteStrategyA());
        context.ExecuteStrategy();

        context.SetStrategy(new ConcreteStrategyB());
        context.ExecuteStrategy();

        context.SetStrategy(new ConcreteStrategyC());
        context.ExecuteStrategy();
    }
}

