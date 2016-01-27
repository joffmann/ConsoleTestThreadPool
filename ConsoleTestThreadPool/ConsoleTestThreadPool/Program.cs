using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleTestThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            const int FibonacciCalculations = 10;

            // One event is used for each Fibonacci object.
            ManualResetEvent[] doneEvents = new ManualResetEvent[FibonacciCalculations];
            Fibonacci[] fibArray = new Fibonacci[FibonacciCalculations];
            Random r = new Random();

            // Configure and start threads using ThreadPool.
            Debug.WriteLine("launching {0} tasks...", FibonacciCalculations);

            for (int i = 0; i < FibonacciCalculations; i++) {
                doneEvents[i] = new ManualResetEvent(true);
            }

            for (int i = 0; i < 100; i++) {
                int threadNr = WaitHandle.WaitAny(doneEvents);
                doneEvents[threadNr] = new ManualResetEvent(false);
                Fibonacci f = new Fibonacci(r.Next(20, 40), doneEvents[threadNr]);
                fibArray[threadNr] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, threadNr);
                Debug.WriteLine("Fibonacci({0}) Thread:{1} Sequens:{2}", f.N, threadNr,i);
            }

            // Wait for all threads in pool to calculate.
            //WaitHandle.WaitAll(doneEvents);
            Debug.WriteLine("All calculations are complete.");
        }
    }
}
