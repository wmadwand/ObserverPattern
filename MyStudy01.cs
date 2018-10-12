using System;
using System.Collections.Generic;

namespace ObserverMS01
{
    public interface IObserver
    {
        void Update(Object obj);
    }

    public interface IObservable
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void Notify();
    }

    public class CurrencyInfo
    {
        public int USD;
        public int Eur;
    }

    public class Stock : IObservable
    {
        private List<IObserver> _observers;
        private CurrencyInfo _currencyInfo;

        public Stock()
        {
            _observers = new List<IObserver>();
            _currencyInfo = new CurrencyInfo();
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver item in _observers)
            {
                item.Update(_currencyInfo);
            }
        }

        public void RunMarket()
        {
            _currencyInfo.USD = 45;
            _currencyInfo.Eur = 75;
            Notify();
        }
    }

    public class Broker : IObserver
    {
        public string name;

        private IObservable _subject;

        public Broker(string name, IObservable subject)
        {
            this.name = name;
            _subject = subject;

            _subject.AddObserver(this);
        }

        public void Update(object obj)
        {
            CurrencyInfo info = (CurrencyInfo)obj;

            if (info.USD > 30)
            {
                Console.WriteLine("Broker is saling");
            }
        }

        public void StopTrade()
        {
            _subject.RemoveObserver(this);
            _subject = null;
        }
    }

    public class Bank : IObserver
    {
        public string name;
        private IObservable _subject;

        public Bank(string name, IObservable subject)
        {
            this.name = name;
            _subject = subject;
            _subject.AddObserver(this);
        }

        public void Update(object obj)
        {
            CurrencyInfo info = (CurrencyInfo)obj;

            if (info.USD > 30)
            {
                Console.WriteLine("Bank is buying");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stock stock = new Stock();

            Broker broker = new Broker("Nicolas", stock);
            Bank bank = new Bank("Swiss", stock);

            stock.RunMarket();

            broker.StopTrade();

            stock.RunMarket();

            Console.ReadKey();
        }
    }
}
