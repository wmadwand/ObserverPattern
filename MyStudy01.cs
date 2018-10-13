using System;
using System.Collections.Generic;

namespace ObserverMS01
{
    // Observer
    public interface IObserver
    {
        void Update(Object obj);
    }

    // Subject
    public interface IObservable
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
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
            _observers.ForEach(x => x.Update(_currencyInfo));
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

            _subject.Attach(this);
        }

        // PUSH-model
        public void Update(object obj)
        {
            CurrencyInfo info = (CurrencyInfo)obj;

            if (info.USD > 30)
            {
                Console.WriteLine("Broker is saling");
            }
        }

        // PULL-model
        public void Update(Stock stock)
        {
            // pull some required info from stock parameter
            // instead of having all the info all the time with no purpose
        }

        public void StopTrade()
        {
            _subject.Detach(this);
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
            _subject.Attach(this);
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
