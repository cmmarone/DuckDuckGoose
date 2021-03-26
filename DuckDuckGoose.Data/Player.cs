using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckGoose.Data
{
    public enum Status { Duck, Fox, Winner }
    public enum Position { DuckOne = 1, DuckTwo, DuckThree, DuckFour, DuckFive, DuckSix, DuckSeven, DuckEight, FoxOne = 11, FoxTwo, FoxThree, FoxFour, FoxFive, FoxSix, FoxSeven, FoxEight, GooseOne = 21, GooseTwo, GooseThree, GooseFour, GooseFive, GooseSix, GooseSeven, GooseEight }
    public class Player
    { 
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Speed { get; set; }
        public Status Status { get; set; }
        public Position Position { get; set; }
        public bool IsHumanPlayer { get; set; }
        public int FoxesCaught { get; set; }

        public Player() { }
        public Player(string name, string avatar, Status status, Position position, bool isHumanPlayer)
        {
            Name = name;
            Avatar = avatar;
            Speed = RandomSpeedGenerator.GenerateRandomSpeed();
            Status = status;
            Position = position;
            IsHumanPlayer = isHumanPlayer;
            FoxesCaught = 0;
        }
    }
}
