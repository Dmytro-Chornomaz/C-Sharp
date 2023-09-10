namespace DesignPatterns.Patterns
{
    public class Adapter
    {
        // The Target defines the domain-specific interface used by the client code.
        public interface ITarget
        {
            string GetRequest();
        }

        // The Adaptee contains some useful behavior, but its interface is
        // incompatible with the existing client code. The Adaptee needs some
        // adaptation before the client code can use it.
        public class Adaptee
        {
            public string GetSpecificRequest()
            {
                return "Specific request.";
            }
        }

        // The Adapter makes the Adaptee's interface compatible with the Target's
        // interface.
        public class MyAdapter : ITarget
        {
            private readonly Adaptee _adaptee;

            public MyAdapter(Adaptee adaptee)
            {
                _adaptee = adaptee;
            }

            public string GetRequest()
            {
                return $"This is '{_adaptee.GetSpecificRequest()}'";
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                Adaptee adaptee = new Adaptee();
                ITarget target = new MyAdapter(adaptee);

                Console.WriteLine("Adaptee interface is incompatible with the client.");
                Console.WriteLine("But with adapter client can call it's method.");

                Console.WriteLine(target.GetRequest());
            }
        }
    }
}
