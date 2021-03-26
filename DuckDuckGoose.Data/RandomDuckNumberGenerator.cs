using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckGoose.Data
{
    public static class RandomDuckNumberGenerator
    {
        public static int GenerateRandomDuckNumber()
        {
            System.Threading.Thread.Sleep(500);
            Random rng = new Random();
            int randomNumberOfDucks = rng.Next(2, 9); //2 to 8 "ducks"
            return randomNumberOfDucks;
        }
    }
}
