using System.Diagnostics;
using System.Threading;

namespace ConsoleTestThreadPool
{
    public class Fibonacci
    {
        private int _n;
        private int _fibOfN;
        private ManualResetEvent _doneEvent;

        public int N { get { return _n; } }
        public int FibOfN { get { return _fibOfN; } }

        // Constructor.
        public Fibonacci(int n, ManualResetEvent doneEvent)
        {
            _n = n;
            _doneEvent = doneEvent;
        }

        // Wrapper method for use with thread pool.
        public void ThreadPoolCallback(object threadContext)
        {
            int threadIndex = (int)threadContext;
            Debug.WriteLine("thread {0} started...", threadIndex);
            _fibOfN = Calculate(_n);
            Debug.WriteLine("thread {0} result calculated...", threadIndex);
            _doneEvent.Set();
        }

        // Recursive method that calculates the Nth Fibonacci number.
        public int Calculate(int n)
        {
            if (n <= 1) {
                return n;
            }

            return Calculate(n - 1) + Calculate(n - 2);
        }
    }

}
