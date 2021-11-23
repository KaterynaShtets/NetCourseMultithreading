/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class Program
    {
        private static readonly List<int> SharedCollection = new List<int>();
        private static readonly object LockObject = new object();
        private static bool _isCollectionModified;
        private static int _printedCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var addTask = Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    var valueAdded = false;
                    while (!valueAdded)
                    {
                        valueAdded = Add(i);
                    }
                }
            });

            Task.Run(() =>
            {
                while (_printedCount != 10)
                {
                    Print();
                }
            });

            addTask.Wait();
        }

        private static void Print()
        {
            lock (LockObject)
            {
                if (!_isCollectionModified)
                {
                    return;
                }

                Console.WriteLine(string.Join(", ", SharedCollection.Select(x => x.ToString())));
                _printedCount += 1;
                _isCollectionModified = false;
            }
        }

        private static bool Add(int value)
        {
            lock (LockObject)
            {
                if (_isCollectionModified)
                {
                    return !_isCollectionModified;
                }

                SharedCollection.Add(value);
                _isCollectionModified = true;
                return _isCollectionModified;
            }
        }
    }
}
