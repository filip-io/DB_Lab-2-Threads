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
                new Car("TurtleVan"),
                new Car("Batmobile"),
                new Car("Gadgetmobile"),
                new Car("LandRaider")
            };

            Console.Title = "THE RACE OF THE AGES!";
            Console.CursorVisible = false;

            Console.WriteLine("\n\t  >>> WELCOME TO THE RACE OF THE AGES! <<<");
            Console.WriteLine("\n\t     Contestants lining up for start... ");

            Thread.Sleep(2000);

            List<Task> raceTasks = cars
                .Select(car =>
            {
                // Print message to display starting contestants
                Console.Write($"\n\t\t\t{car.Name} \t");

                // Start a 'Task' for each car with the RaceAsync method to run asynchronously
                return Task.Run(() => car.RaceAsync());
            })
            .ToList();

            Console.WriteLine("\n\n\n\t\t>>> THE RACE HAS STARTED! <<<");
            Console.WriteLine("\n     [ Press ENTER during the race to display current status ] \n");

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
                 * Assign a reference to the first Car object that passes the finish line to variable 'winner' which is of type 'Car'. */
                Car? winner = cars.FirstOrDefault(car => car.Distance >= 10000);

                /* Check if any car has finished the race. Will only assign one winner since the TryAdd method of ConcurrentDictionary
                 * will only add "Winner" if it's not already assigned */
                if (winner != null && raceStatus.TryAdd("Winner", winner))
                {
                    Console.WriteLine($"\n\n\t~~~ {winner.Name} IS THE WINNER! ~~~\n");
                }

                if (raceTasks.All(t => t.IsCompleted))
                {
                    Console.WriteLine("\n\tALL CARS FINISHED!");
                    break;
                }
            }
            // Use await to make sure all tasks are completed before exiting
            await Task.WhenAll(raceTasks);

            Console.ReadKey();
        }
    }
}