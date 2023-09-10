using static DesignPatterns.Patterns.Adapter;
using static DesignPatterns.Patterns.Builder;
using static DesignPatterns.Patterns.Observer;
using DesignPatterns.Patterns;

//************************************************************************************
//Factory Method

//new FactoryMethod.Client().Main();

//************************************************************************************
//Singleton

Console.WriteLine(
    "{0}\n{1}\n\n{2}\n",
    "If you see the same value, then singleton was reused (yay!)",
    "If you see different values, then 2 singletons were created (booo!!)",
    "RESULT:"
);

Thread process1 = new Thread(() =>
{
    TestSingleton("FOO");
});
Thread process2 = new Thread(() =>
{
    TestSingleton("BAR");
});

process1.Start();
process2.Start();

process1.Join();
process2.Join();

static void TestSingleton(string value)
{
    Singleton singleton = Singleton.GetInstance(value);
    Console.WriteLine(singleton.Value);
}

//************************************************************************************
//Facade

//Subsystem1 subsystem1 = new Subsystem1();
//Subsystem2 subsystem2 = new Subsystem2();
//Facade facade = new Facade(subsystem1, subsystem2);
//Client.ClientCode(facade);

//************************************************************************************
//Strategy

//var context = new Context();

//Console.WriteLine("Client: Strategy is set to normal sorting.");
//context.SetStrategy(new ConcreteStrategyA());
//context.DoSomeBusinessLogic();

//Console.WriteLine();

//Console.WriteLine("Client: Strategy is set to reverse sorting.");
//context.SetStrategy(new ConcreteStrategyB());
//context.DoSomeBusinessLogic();

//************************************************************************************
//Observer

//var subject = new Subject();
//var observerA = new ConcreteObserverA();
//subject.Attach(observerA);

//var observerB = new ConcreteObserverB();
//subject.Attach(observerB);

//subject.SomeBusinessLogic();
//subject.SomeBusinessLogic();

//subject.Detach(observerB);

//subject.SomeBusinessLogic();

//************************************************************************************
//Builder

//var director = new Director();
//var builder = new ConcreteBuilder();
//director.Builder = builder;

//Console.WriteLine("Standard basic product:");
//director.BuildMinimalViableProduct();
//Console.WriteLine(builder.GetProduct().ListParts());

//Console.WriteLine("Standard full featured product:");
//director.BuildFullFeaturedProduct();
//Console.WriteLine(builder.GetProduct().ListParts());

//Console.WriteLine("Custom product:");
//builder.BuildPartA();
//builder.BuildPartC();
//Console.Write(builder.GetProduct().ListParts());

//************************************************************************************
//Adapter

//Adaptee adaptee = new Adaptee();
//ITarget target = new MyAdapter(adaptee);

//Console.WriteLine("Adaptee interface is incompatible with the client.");
//Console.WriteLine("But with adapter client can call it's method.");

//Console.WriteLine(target.GetRequest());

//************************************************************************************
//Mediator

//Component1 component1 = new Component1();
//Component2 component2 = new Component2();
//new ConcreteMediator(component1, component2);

//Console.WriteLine("Client triggers operation A.");
//component1.DoA();

//Console.WriteLine();

//Console.WriteLine("Client triggers operation D.");
//component2.DoD();


