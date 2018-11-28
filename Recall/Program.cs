using System;
using System.Collections.Generic;

namespace Recall
{
    public interface IObserver
    {
        void Update(object obj);
    }

    public interface IObservable
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class CurrencyInfo
    {
        public float USD = 60;
        public float RUB = 20;
    }

    public class Stock : IObservable
    {
        private List<IObserver> _observers = new List<IObserver>();
        private CurrencyInfo _currencyInfo;

        public Stock(CurrencyInfo currencyInfo)
        {
            _currencyInfo = currencyInfo;
        }

        void IObservable.Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        void IObservable.Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            _observers.ForEach(item => item.Update(_currencyInfo));
        }

        public void ChangeRate()
        {
            _currencyInfo.USD = 32;
            _currencyInfo.RUB = 60;

            Notify();
        }
    }

    public class Broker : IObserver
    {
        private IObservable _stock;

        public Broker(IObservable stock)
        {
            _stock = stock;
            _stock.Attach(this);
        }

        void IObserver.Update(object obj)
        {
            CurrencyInfo currencyInfo = (CurrencyInfo)obj; // PUSH-model

            if (currencyInfo.USD < 40)
            {
                Console.WriteLine("Broker is selling USD");
            }
        }

        public void StopTrade()
        {
            _stock.Detach(this);
            _stock = null;

            Console.WriteLine("Broker got out");
        }
    }

    public class Bank : IObserver
    {
        private IObservable _stock;

        public Bank(IObservable stock)
        {
            _stock = stock;
            _stock.Attach(this);
        }

        void IObserver.Update(object obj)
        {
            CurrencyInfo currencyInfo = (CurrencyInfo)obj; // PUSH-model

            if (currencyInfo.USD < 40)
            {
                Console.WriteLine("Bank is selling USD");
            }
        }

        public void StopTrade()
        {
            _stock.Detach(this);
            _stock = null;

            Console.WriteLine("Bank got out");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CurrencyInfo currencyInfo = new CurrencyInfo();

            Stock stock = new Stock(currencyInfo);

            Broker broker = new Broker(stock);
            Bank bank = new Bank(stock);

            stock.ChangeRate();
            broker.StopTrade();

            stock.ChangeRate();
            bank.StopTrade();

            stock.ChangeRate();
        }
    }
}
