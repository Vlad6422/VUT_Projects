/*Explanation:
Singleton Class:

sealed keyword: Ensures the class cannot be inherited.
_instance: A private static variable to hold the single instance of the class.
_lock: A private static object used for thread - safe locking.
Private constructor: Ensures that the class cannot be instantiated from outside.
Instance property: Provides global access to the single instance of the class. 
Uses double-checked locking to ensure thread safety when creating the instance.

Client Code:

The Program class demonstrates how to access the Singleton instance. 
It retrieves the instance using the Instance property and checks if multiple calls to Instance return the same object.
Finally, it calls a method on the Singleton instance to show it is functioning.
This implementation ensures that there is only one instance of the Singleton class
and provides a global point of access to it, making it thread-safe and lazy-loaded.*/
public sealed class Singleton
{
    private static Singleton _instance = null;
    private static readonly object _lock = new object();

    // Private constructor to prevent direct instantiation
    private Singleton()
    {
    }

    public static Singleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }
                return _instance;
            }
        }
    }

    public void DoSomething()
    {
        Console.WriteLine("Singleton instance is working.");
    }
}

// Client Code
public class Program
{
    public static void Main(string[] args)
    {
        Singleton singleton1 = Singleton.Instance;
        Singleton singleton2 = Singleton.Instance;

        if (singleton1 == singleton2)
        {
            Console.WriteLine("Both instances are the same.");
        }

        singleton1.DoSomething();
    }
}
