using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DB_Lab_2_Threads
{
    internal class Program
    {
        static async Task Main()
        {
            List<Car> cars = new List<Car>
            {
                new Car("Skoda"),
                new Car("KIA"),
                new Car("Audi"),
                new Car("BMW")
            };

            Console.WriteLine("\n\t>>> THE RACE HAS STARTED! <<<");

            // Start all car tasks simultaneously using Task.Run
            List<Task> raceTasks = cars.Select(car => Task.Run(() => car.RaceAsync())).ToList();

            // ConcurrentDictionary to track race status
            ConcurrentDictionary<string, Car> raceStatus = new ConcurrentDictionary<string, Car>();

            // Check race status when the user presses Enter
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("\n\tRace Status:");
                    Console.WriteLine("\t------------------");
                    foreach (var car in cars)
                    {
                        Console.WriteLine($"\t{car.Name}: \tDistance: {car.Distance} km \tSpeed: {car.Speed} km/h");
                    }
                }

                // Check if any car has finished the race
                Car winner = cars.FirstOrDefault(car => car.Distance >= 10000);
                if (winner != null && raceStatus.TryAdd("Winner", winner))
                {
                    Console.WriteLine($"\n\n\t~~~ {winner.Name} IS THE WINNER! ~~~\n");
                }

                // Check if all cars have finished the race
                if (raceTasks.All(t => t.IsCompleted))
                {
                    Console.WriteLine("\n\tALL CARS FINISHED!");
                    break;
                }
            }

            // Make sure all tasks are completed before exiting
            await Task.WhenAll(raceTasks);

            Console.ReadKey();
        }
    }
}
