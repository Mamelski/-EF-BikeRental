using System;
using System.Threading;
using System.Threading.Tasks;

namespace Intel.BikeRental.ConsoleClient
{
    public class AsyncTests
    {
        public void Start()
        {
            // var task = Task.Run(() => this.LongTask());
            // task.Wait();

            this.DoWorkAsync();
            Console.WriteLine("Main thread finished.");
        }

        public void CalculateTest()
        {
            var result1 = this.Calculate();
            var result2 = this.Calculate();

            Console.WriteLine($"SUM: {result1 + result2}");
        }

        public async void CalculateAsyncTest()
        {
            var result1 = await this.CalculateAsync();
            var result2 = await this.CalculateAsync();

            Console.WriteLine($"SUM: {result1 + result2}");
        }

        public async void CalculateAsyncEntityTest()
        {
            var result1 = await this.CalculateAsync();
            var result2 = await this.CalculateAsync();

            using (var context = new BikeRental.DAL.BikeRentalContext())
            {
                var rental = await context.Rentals.FindAsync(2);
                rental.Cost = result1 + result2;

                await context.SaveChangesAsync();

                Console.WriteLine($"Entity changed.");
            }
        }

        public void CalculateAsyncTestTasks()
        {
            var task1 = this.CalculateAsync();
            var task2 = this.CalculateAsync();

            Task.WaitAll();

            Console.WriteLine($"SUM: {task1.Result + task2.Result}");
        }

        private void DoWork()
        {
            Console.WriteLine("Working...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine("Success");
        }

        private Task DoWorkAsync()
        {
            return Task.Run(() => DoWork());
        }

        private Task<decimal> CalculateAsync()
        {
            return Task.Run(() => this.Calculate());
        }

        private decimal Calculate()
        {
            Console.WriteLine("Calculating...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            return 100m;
        }
    }
}
