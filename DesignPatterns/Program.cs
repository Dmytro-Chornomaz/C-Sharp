using DesignPatterns;
using static DesignPatterns.Adapter;
using static DesignPatterns.Builder;
using static DesignPatterns.Observer;

//************************************************************************************
//Factory Method

//new FactoryMethod.Client().Main();

//************************************************************************************
//Singleton

//Singleton s1 = Singleton.GetInstance();
//Singleton s2 = Singleton.GetInstance();

//if (s1 == s2)
//{
//    Console.WriteLine("Singleton works, both variables contain the same instance.");
//}
//else
//{
//    Console.WriteLine("Singleton failed, variables contain different instances.");
//}

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

Adaptee adaptee = new Adaptee();
ITarget target = new MyAdapter(adaptee);

Console.WriteLine("Adaptee interface is incompatible with the client.");
Console.WriteLine("But with adapter client can call it's method.");

Console.WriteLine(target.GetRequest());

