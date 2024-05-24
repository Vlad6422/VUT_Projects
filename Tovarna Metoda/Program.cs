using System;
// All Console.WriteLine used to show how program is running,
// please run app to see the result,
// stdout is not part of implementation.
namespace TovarnaMetoda
{
    // Interface defining the structure of a product
    interface IProduct
    {
        string Name { get; set; }
    }

    // Abstract class representing the creator in the factory method pattern
    abstract class Creator
    {
        // Abstract method defining the factory method
        public abstract IProduct FactoryMethod();

        // Method representing an operation that uses the factory method to create a product
        public void AnOperation()
        {
            Console.WriteLine("\n\nCreator.AnOperation()");
            IProduct Product = FactoryMethod();
            Console.WriteLine("IProduct Product = FactoryMethod()");
            if (Product.Name == "Pizza")
            {
                Console.WriteLine("Pizza is cooking, type is " + Product.GetType());
            }
            else if (Product.Name == "Pasta")
            {
                Console.WriteLine("Pasta is cooking, type is " + Product.GetType());
            }
            Console.WriteLine("You can see that User used only abstract class Creator and Intefrace IProduct,but get " + Product.GetType());
        }
    }

    // Concrete product class representing a pizza
    class Pizza : IProduct
    {
        public string Name { get; set; }
    }

    // Concrete product class representing a pasta
    class Pasta : IProduct
    {
        public string Name { get; set; }
    }

    // Concrete creator class for creating pizzas
    class MakerPizza : Creator
    {
        // Implementation of the factory method to create a pizza
        public override IProduct FactoryMethod()
        {
            return new Pizza { Name = "Pizza" };
        }
    }

    // Concrete creator class for creating pastas
    class MakerPasta : Creator
    {
        // Implementation of the factory method to create a pasta
        public override IProduct FactoryMethod()
        {
            return new Pasta { Name = "Pasta" };
        }
    }

    // Main program class
    public class Program
    {

        // Main method where the client code interacts with the factory method pattern
        public static void Main(string[] args)
        {
            // Create a pizza creator
            Creator pizzaCreator = new MakerPizza();
            // Perform an operation to create a pizza
            pizzaCreator.AnOperation();

            // Create a pasta creator
            Creator pastaCreator = new MakerPasta();
            // Perform an operation to create a pasta
            pastaCreator.AnOperation();

        }
    }
}
