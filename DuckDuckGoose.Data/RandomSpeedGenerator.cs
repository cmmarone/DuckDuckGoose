using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckGoose.Data
{
    public static class RandomSpeedGenerator
    {
        public static int GenerateRandomSpeed()
        {
            System.Threading.Thread.Sleep(500);
            Random rng = new Random();
            int randomSpeed = rng.Next(20, 26); //20 to 25 spacebar entries in 5 seconds
            return randomSpeed;
        }
    }
}
