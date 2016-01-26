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
                doneEvents[i] = new ManualResetEvent(false);
                Fibonacci f = new Fibonacci(r.Next(20, 40), doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }
            int ready1 = WaitHandle.WaitAny(doneEvents);
            Debug.WriteLine("Nr " + ready1 + " är klar");
            doneEvents[ready1] = new ManualResetEvent(false);
            Fibonacci f2 = new Fibonacci(r.Next(20, 40), doneEvents[ready1]);
            fibArray[ready1] = f2;
            ThreadPool.QueueUserWorkItem(f2.ThreadPoolCallback, ready1);

            // Wait for all threads in pool to calculate.
            WaitHandle.WaitAll(doneEvents);
            Debug.WriteLine("All calculations are complete.");

            // Display the results.
            for (int i = 0; i < FibonacciCalculations; i++) {
                Fibonacci f = fibArray[i];
                Debug.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
            }
        }
    }
}
