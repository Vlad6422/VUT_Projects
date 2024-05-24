using System;

namespace Abstraktni_Tovarna
{
    /// <summary>
    /// Interface defining the structure of a pizza
    /// </summary>
    interface IPizza
    {
        string Name { get; }
        void Prepare();
    }

    /// <summary>
    /// Interface defining the structure of a pasta
    /// </summary>
    interface IPasta
    {
        string Name { get; }
        void Cook();
    }

    /// <summary>
    /// Concrete class representing Margherita Pizza
    /// </summary>
    class MargheritaPizza : IPizza
    {
        /// <summary>
        /// Name of the pizza
        /// </summary>
        public string Name => "Margherita Pizza";

        /// <summary>
        /// Prepare the Margherita Pizza
        /// </summary>
        public void Prepare()
        {
            Console.WriteLine("Preparing Margherita Pizza.");
        }
    }

    /// <summary>
    /// Concrete class representing Carbonara Pasta
    /// </summary>
    class CarbonaraPasta : IPasta
    {
        /// <summary>
        /// Name of the pasta
        /// </summary>
        public string Name => "Carbonara Pasta";

        /// <summary>
        /// Cook the Carbonara Pasta
        /// </summary>
        public void Cook()
        {
            Console.WriteLine("Cooking Carbonara Pasta.");
        }
    }

    /// <summary>
    /// Interface defining the structure of an Italian cuisine factory
    /// </summary>
    interface IItalianCuisineFactory
    {
        IPizza CreatePizza();
        IPasta CreatePasta();
    }

    /// <summary>
    /// Concrete implementation of the Italian cuisine factory
    /// </summary>
    class ItalianCuisineFactory : IItalianCuisineFactory
    {
        /// <summary>
        /// Creates a Margherita Pizza
        /// </summary>
        /// <returns>Margherita Pizza instance</returns>
        public IPizza CreatePizza()
        {
            return new MargheritaPizza();
        }

        /// <summary>
        /// Creates a Carbonara Pasta
        /// </summary>
        /// <returns>Carbonara Pasta instance</returns>
        public IPasta CreatePasta()
        {
            return new CarbonaraPasta();
        }
    }

    /// <summary>
    /// Main program entry point
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            // Create an instance of Italian cuisine factory
            IItalianCuisineFactory factory = new ItalianCuisineFactory();

            // Create and prepare a Margherita Pizza
            IPizza pizza = factory.CreatePizza();
            pizza.Prepare();
            Console.WriteLine($"Created: {pizza.Name}");

            // Create and cook Carbonara Pasta
            IPasta pasta = factory.CreatePasta();
            pasta.Cook();
            Console.WriteLine($"Created: {pasta.Name}");
        }
    }
}
