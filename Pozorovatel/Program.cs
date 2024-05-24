using System;
using System.Collections.Generic;

// Subject Interface
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// Observer Interface
public interface IObserver
{
    void Update(ISubject subject);
}

// Concrete Subject
public class ConcreteSubject : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private int _state;

    public int State
    {
        get => _state;
        set
        {
            _state = value;
            Notify();
        }
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}

// Concrete Observer A
public class ConcreteObserverA : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is ConcreteSubject concreteSubject && concreteSubject.State < 3)
        {
            Console.WriteLine("ConcreteObserverA: Reacted to the event.");
        }
    }
}

// Concrete Observer B
public class ConcreteObserverB : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is ConcreteSubject concreteSubject && concreteSubject.State >= 3)
        {
            Console.WriteLine("ConcreteObserverB: Reacted to the event.");
        }
    }
}

// Client Code
public class Program
{
    public static void Main(string[] args)
    {
        ConcreteSubject subject = new ConcreteSubject();

        ConcreteObserverA observerA = new ConcreteObserverA();
        subject.Attach(observerA);

        ConcreteObserverB observerB = new ConcreteObserverB();
        subject.Attach(observerB);

        Console.WriteLine("Changing subject state to 2:");
        subject.State = 2; // Only ConcreteObserverA reacts

        Console.WriteLine("Changing subject state to 3:");
        subject.State = 3; // Only ConcreteObserverB reacts
    }
}
