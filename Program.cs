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

            /* Start all car tasks simultaneously using Task.Run() on 
             * each element of the 'cars' list to run tasks asynchronously.
             * Convert result of 'Select' operation: IEnumerable<Task>, to a List<Task>. */
            List<Task> raceTasks = cars.Select(car => Task.Run(() => car.RaceAsync())).ToList();

            /* Use ConcurrentDictionary to track race status. This thread-safe collection can be accessed 
             * concurrecntly by multiple threads without explicit locking. */
            ConcurrentDictionary<string, Car> raceStatus = new ConcurrentDictionary<string, Car>();

            // Check race status when the user presses keyboard key 'Enter'
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


                /* Check using LINQ for the first car in the 'cars' list that has passed the finish line (distance > 10000).
                 * Assign a reference to the first Car object that passes the finish line to variable 'winner' which is of type 'Car'.
                 * Make winner nullable by adding '?'*/
                Car? winner = cars.FirstOrDefault(car => car.Distance >= 10000);

                /* Check if any car has finished the race. Will only assign one winner since the TryAdd method of ConcurrentDictionary
                 * will only add "Winner" if it's not already assigned */
                if (winner != null && raceStatus.TryAdd("Winner", winner))
                {
                    Console.WriteLine($"\n\n\t~~~ {winner.Name} IS THE WINNER! ~~~\n");
                }

                // Check using LINQ and a lambda expression if all cars have finished the race
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
