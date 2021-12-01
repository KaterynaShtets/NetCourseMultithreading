/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int CountOfNumbers = 10;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();
            var random = new Random();
            // feel free to add your code
            var chain = Task.Run(() =>
            {
                var numbers = new int[10];
                for (var i = 0; i < CountOfNumbers; i++)
                {
                    numbers[i] = random.Next(-100,100);
                }
                return numbers;
            }).ContinueWith(resultOfFirstTask =>
            {
                var array = resultOfFirstTask.Result;
                var randomNumber = random.Next(-100,100);
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] *= randomNumber;
                }
                return array;
            }).ContinueWith(resultOfSecondArray =>
            {
                var array = resultOfSecondArray.Result;
                var sortedArray = array.OrderBy(x=>x).ToArray();
                return sortedArray;
            }).ContinueWith(resultOfThirdTask =>
            {
                var array = resultOfThirdTask.Result;
                Console.WriteLine($"Average value: {array.Average()}");
            });
            chain.Wait();
            Console.ReadLine();
        }
    }
}
