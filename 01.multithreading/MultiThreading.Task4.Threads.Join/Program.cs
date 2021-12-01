/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static int _runCount;
        private static readonly Semaphore _semaphore = new Semaphore(0, 9);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();

            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            _runCount = 0;
            CreateThreadByTask(54);

            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            _runCount = 0;
            CreateThreadByThreadPool(54);

            Console.WriteLine("Finish");
            Console.ReadLine();
        }

        private static void CreateThreadByTask(int n)
        {
            if (_runCount == 10)
                return;

            _runCount++;

            var thread = new Thread(() =>
            {
                Console.WriteLine(n);
                --n;
                CreateThreadByTask(n);
            });

            thread.Start();
            thread.Join();
        }

        private static void CreateThreadByThreadPool(int n)
        {
            if (_runCount == 10)
                return;

            _runCount++;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine(n);
                --n;

                CreateThreadByThreadPool(n);

                _semaphore.Release();
            });

            _semaphore.WaitOne();
        }
    }
}
