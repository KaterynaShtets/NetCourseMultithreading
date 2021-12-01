/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var anyParentResultTask = Task.FromCanceled(new CancellationToken(true))
                .ContinueWith(cancelledTask =>
                {
                    Console.WriteLine(cancelledTask.Status);
                    throw new Exception();
                }).ContinueWith(faultedTask =>
                {
                    Console.WriteLine(faultedTask.Status);
                    return string.Empty;
                }).ContinueWith(successfulTask =>
                {
                    Console.WriteLine(successfulTask.Status);
                });
            anyParentResultTask.Wait();

            var notSuccessParentTask = Task.FromCanceled(new CancellationToken(true))
            .ContinueWith(cancelledTask =>
            {
                Console.WriteLine("This continuation triggers when antecedent finished without success");
            }, TaskContinuationOptions.NotOnRanToCompletion);
            notSuccessParentTask.Wait();

            var failedParentTaskSameThread = Task.Run(() =>
            {
                Console.WriteLine("Parent thread Id: " + Thread.CurrentThread.ManagedThreadId);
                throw new Exception();
            }).ContinueWith(failedTask =>
            {
                Console.WriteLine("This continuation triggers when antecedent finished with fail");
                Console.WriteLine("Child thread Id: " + Thread.CurrentThread.ManagedThreadId);
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            failedParentTaskSameThread.Wait();

            var cancelledParentTaskOutsideThreadPool = Task.FromCanceled(new CancellationToken(true))
                .ContinueWith(failedTask =>
                {
                    Console.WriteLine("Is thread pool thread: " + Thread.CurrentThread.IsThreadPoolThread);
                }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
            cancelledParentTaskOutsideThreadPool.Wait();
        }
    }
}
