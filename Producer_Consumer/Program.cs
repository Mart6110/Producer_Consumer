using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Producer_Consumer
{
    public class Program
    {
        static Queue<int> _item = new Queue<int>();
        static void Main(string[] args)
        {
            // Creating Threads
            Thread producer = new Thread(new ThreadStart(Producer));
            Thread consumer = new Thread(new ThreadStart(Consumer));
            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
        }

        public static void Producer()
        {
            while(true)
            {
                Monitor.Enter(_item);
                try
                {
                    // If we have no items in our queue we run a for loop that fill our queue up with 10 items
                    if(_item.Count == 0)
                    {
                        for (int item = 0; item < 10; item++)
                        {
                            _item.Enqueue(item);
                            Console.WriteLine("Producer has made item: " + _item.Count);
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        Console.WriteLine("Producer Waits...");
                        Monitor.PulseAll(_item);
                    }
                }
                finally
                {
                    Monitor.Exit(_item);
                }
            }
        }

        public static void Consumer()
        {
            while(true)
            {
                    Monitor.Enter(_item);
                    try
                    {
                        while (_item.Count == 0)
                        {
                            Monitor.Wait(_item);
                        }

                        Console.WriteLine("Consumer has bought item: " + _item.Count);
                        _item.Dequeue();
                        if (_item.Count == 0)
                        {
                            Console.WriteLine("Consumer waits...");
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    finally
                    {
                        Monitor.Exit(_item);
                    }
            }
        }
    }
}
