using System;

namespace Adapter
{
    /// <summary>
    /// Interface representing the target that the client code expects to interact with.
    /// </summary>
    public interface ITarget
    {
        string GetRequest();
    }

    /// <summary>
    /// Class representing the adaptee, which has a specific request method.
    /// </summary>
    public class Adaptee
    {
        /// <summary>
        /// Method representing the specific request that the adaptee can handle.
        /// </summary>
        /// <returns>A string representing the specific request.</returns>
        public string GetSpecificRequest()
        {
            return "Specific request";
        }
    }

    /// <summary>
    /// Adapter class that adapts the Adaptee to the ITarget interface.
    /// </summary>
    public class Adapter : ITarget
    {
        private readonly Adaptee _adaptee;

        /// <summary>
        /// Constructor that initializes the adapter with an instance of the Adaptee.
        /// </summary>
        /// <param name="adaptee">The adaptee instance to be adapted.</param>
        public Adapter(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        /// <summary>
        /// Implementation of the GetRequest method from the ITarget interface,
        /// which internally calls the specific request method of the Adaptee.
        /// </summary>
        /// <returns>A string representing the adapted request.</returns>
        public string GetRequest()
        {
            return $"This is '{_adaptee.GetSpecificRequest()}'";
        }
    }

    /// <summary>
    /// Main program entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method where the client code interacts with the adapter.
        /// </summary>
        /// <param name="args">Command line arguments (not used).</param>
        static void Main(string[] args)
        {
            // Create an instance of Adaptee
            Adaptee adaptee = new Adaptee();

            // Create an instance of the adapter, adapting the Adaptee to the ITarget interface
            ITarget target = new Adapter(adaptee);

            // Client code interacts with the ITarget interface
            Console.WriteLine("Client: I can work just fine with the Target interface.");
            Console.WriteLine(target.GetRequest());
        }
    }
}
