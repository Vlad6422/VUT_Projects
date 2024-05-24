namespace Dekorator
{
    /// <summary>
    /// Interface defining the basic operations that all components must implement.
    /// </summary>
    public interface IComponent
    {
        string Operation();
    }

    /// <summary>
    /// Concrete implementation of the IComponent interface.
    /// </summary>
    public class ConcreteComponent : IComponent
    {
        public string Operation()
        {
            return "ConcreteComponent";
        }
    }

    /// <summary>
    /// Abstract decorator class implementing the IComponent interface.
    /// </summary>
    public abstract class Decorator : IComponent
    {
        protected IComponent _component;

        // Constructor to initialize the decorator with a component.
        public Decorator(IComponent component)
        {
            _component = component;
        }

        // Default implementation of Operation, which delegates to the wrapped component.
        public virtual string Operation()
        {
            return _component.Operation();
        }
    }

    /// <summary>
    /// Concrete decorator adding additional behavior to the component.
    /// </summary>
    public class ConcreteDecoratorA : Decorator
    {
        // Constructor to initialize the decorator with a component.
        public ConcreteDecoratorA(IComponent component) : base(component)
        {
        }

        // Overrides the Operation method to add additional behavior.
        public override string Operation()
        {
            return $"ConcreteDecoratorA({_component.Operation()})";
        }
    }

    /// <summary>
    /// Another concrete decorator adding different additional behavior to the component.
    /// </summary>
    public class ConcreteDecoratorB : Decorator
    {
        // Constructor to initialize the decorator with a component.
        public ConcreteDecoratorB(IComponent component) : base(component)
        {
        }

        // Overrides the Operation method to add different additional behavior.
        public override string Operation()
        {
            return $"ConcreteDecoratorB({_component.Operation()})";
        }
    }

    // Main program entry point.
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a simple component.
            IComponent component = new ConcreteComponent();
            Console.WriteLine("Client: I get a simple component:");
            Console.WriteLine(component.Operation());

            // Decorate the component with ConcreteDecoratorA.
            IComponent decoratorA = new ConcreteDecoratorA(component);
            Console.WriteLine("\nClient: Now I've got a decorated component with ConcreteDecoratorA:");
            Console.WriteLine(decoratorA.Operation());

            // Decorate the already decorated component with ConcreteDecoratorB.
            IComponent decoratorB = new ConcreteDecoratorB(decoratorA);
            Console.WriteLine("\nClient: Now I've got a decorated component with ConcreteDecoratorA and ConcreteDecoratorB:");
            Console.WriteLine(decoratorB.Operation());
        }
    }
}
