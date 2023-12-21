using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DB_Lab_2_Threads
{
    internal class Car
    {
        public string Name { get; }
        public int Distance { get; private set; } = 0;
        public int Speed { get; private set; } = 120;

        public Car(string name)
        {
            Name = name;
        }

        public async Task RaceAsync()
        {
            while (Distance < 10000)
            {
                await Task.Delay(6000); // Use Task.Delay to represent the race progress

                Random random = new Random();
                int incident = random.Next(1, 51);

                if (incident == 1) // Chance of occurance 1/50
                {
                    Console.WriteLine($"\n {Name} has no gas left and needs to refuel. Delayed by 3 seconds.");
                    await Task.Delay(3000);
                }
                else if (incident <= 3) // Chance of occurance 2/50
                {
                    Console.WriteLine($"\n {Name} got a flat tire and needs to swap it. Delayed by 2 seconds.");
                    await Task.Delay(2000);
                }
                else if (incident <= 8) // Chance of occurance 5/50
                {
                    Console.WriteLine($"\n {Name} got hit by a bird and needs to clean the windshield. Delayed by 1 second.");
                    await Task.Delay(1000);
                }
                else if (incident <= 18) // Chance of occurance 10/50
                {
                    Console.WriteLine($"\n {Name} has engine problems. Speed reduced by 1 km/h.");
                    Speed--;
                }

                Distance += Speed; // Add to distance property after each iteration completes

                if (Distance >= 10000)
                {
                    Console.WriteLine($"\n\t{Name} has finished!");
                }
            }
        }
    }
}
