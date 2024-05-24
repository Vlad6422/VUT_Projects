using System;

// Abstract Base Class
public abstract class AbstractClass
{
    // Template method
    public void TemplateMethod()
    {
        BaseOperation1();
        RequiredOperation1();
        BaseOperation2();
        RequiredOperation2();
        Hook();
    }

    // These operations already have implementations.
    protected void BaseOperation1()
    {
        Console.WriteLine("AbstractClass says: I am doing the bulk of the work");
    }

    protected void BaseOperation2()
    {
        Console.WriteLine("AbstractClass says: But I let subclasses override some operations");
    }

    // These operations have to be implemented in subclasses.
    protected abstract void RequiredOperation1();
    protected abstract void RequiredOperation2();

    // This is a "hook." Subclasses may override it, but it's optional since the hook provides a default implementation.
    protected virtual void Hook() { }
}

// Concrete Class 1
public class ConcreteClass1 : AbstractClass
{
    protected override void RequiredOperation1()
    {
        Console.WriteLine("ConcreteClass1 says: Implemented Operation1");
    }

    protected override void RequiredOperation2()
    {
        Console.WriteLine("ConcreteClass1 says: Implemented Operation2");
    }

    protected override void Hook()
    {
        Console.WriteLine("ConcreteClass1 says: Overridden Hook");
    }
}

// Concrete Class 2
public class ConcreteClass2 : AbstractClass
{
    protected override void RequiredOperation1()
    {
        Console.WriteLine("ConcreteClass2 says: Implemented Operation1");
    }

    protected override void RequiredOperation2()
    {
        Console.WriteLine("ConcreteClass2 says: Implemented Operation2");
    }
}

// Client Code
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Same client code can work with different subclasses:");

        AbstractClass class1 = new ConcreteClass1();
        class1.TemplateMethod();

        Console.WriteLine();

        AbstractClass class2 = new ConcreteClass2();
        class2.TemplateMethod();
    }
}
