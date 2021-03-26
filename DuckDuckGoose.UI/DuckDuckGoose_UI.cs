using DuckDuckGoose.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DuckDuckGoose.UI
{
    public class DuckDuckGoose_UI
    {
        private readonly PlayerRepo _playerRepo = new PlayerRepo();

        public void Run()
        {
            SeedComputerPlayers();
            GameIntro();
            PlayGame();
        }

        public void PlayGame()
        {
            Player humanPlayer = _playerRepo.GetHumanPlayer();

            BeTheFox(humanPlayer); //player always starts as the fox

            while (true)
            {
                if (humanPlayer.Status == Status.Winner)
                {
                    break;
                }
                if (humanPlayer.Status == Status.Fox)
                {
                    BeTheFox(humanPlayer);
                }
                if (humanPlayer.Status == Status.Duck)
                {
                    BeFowl(humanPlayer);
                }
            }

            Console.Clear();
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine($"                     {humanPlayer}, you are the WINNER!!!");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(@"                            ,----,
                       ___.`      `,
                       `===  D     :
                         `'.      .'
                            )    (                   ,
                           /      \_________________/|
                          /                          |
                         |                           ;
                         |               _____       /
                         |      \       ______7    ,'
                         |       \    ______7     /
                          \       `-,____7      ,'   
                    ^~^~^~^`\                  /~^~^~^~^
                      ~^~^~^ `----------------' ~^~^~^
                     ~^~^~^~^~^^~^~^~^~^~^~^~^~^~^~^~");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("                       ---Press Any Key to Exit---");
            Console.ReadKey();



        }


        public void BeTheFox(Player humanPlayer)
        {
            PrintPlayerPositions();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine($"You are the fox, {humanPlayer.Name}!");
            Console.WriteLine(" ");
            Console.WriteLine("Press D to say \"Duck...\"\n" +
                "Press G to say \"Goose!\"");
            Console.WriteLine(" ");

            bool keepFoxLoopRunning = true;
            int duckCount = 0;
            while (keepFoxLoopRunning)
            {
                ConsoleKeyInfo foxInput = Console.ReadKey();
                if ((foxInput.KeyChar == 'd' || foxInput.KeyChar == 'D') && (duckCount < 20))
                {
                    duckCount = duckCount + 1;
                    int targetIndex = (int)humanPlayer.Position;
                    if (targetIndex >= 11 && targetIndex <= 17) //11-18 are the indexes for fox in Position enum
                    {
                        targetIndex = targetIndex + 1;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        PrintDuckDeclarations(duckCount);
                    }
                    else if (targetIndex == 18)
                    {
                        targetIndex = 11;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        PrintDuckDeclarations(duckCount);
                    }
                }
                else if ((foxInput.KeyChar == 'd' || foxInput.KeyChar == 'D') && (duckCount >= 15)) //15 ducks is enough, let's force goose
                {
                    int targetIndex = (int)humanPlayer.Position;
                    if (targetIndex >= 11 && targetIndex <= 17)
                    {
                        targetIndex = targetIndex + 1;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        break;
                    }
                    else if (targetIndex == 18)
                    {
                        targetIndex = 11;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        break;
                    }
                }
                else if (foxInput.KeyChar == 'g' || foxInput.KeyChar == 'G')
                {
                    int targetIndex = (int)humanPlayer.Position;
                    if (targetIndex >= 11 && targetIndex <= 17) //11-18 are the indexes for fox in Position enum
                    {
                        targetIndex = targetIndex + 1;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        break;
                    }
                    else if (targetIndex == 18)
                    {
                        targetIndex = 11;
                        humanPlayer.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                        PrintPlayerPositions();
                        break;
                    }
                }
            }

            int foxIndex = (int)humanPlayer.Position;
            int goosedPlayerIndex = foxIndex - 10;
            Player becameGoose = _playerRepo.GetPlayerByIndex(goosedPlayerIndex);
            Console.WriteLine("GOOSE!!!");
            Console.WriteLine(" ");
            Console.WriteLine($"Try and take {becameGoose.Name}'s spot in the circle!\n" +
                "Mash the spacebar to run.");
            Console.ReadKey();

            int spacePresses = ChaseSequence(becameGoose);
            if ((spacePresses + 5) >= becameGoose.Speed) //give a 5-step headstart to whoever is the fox
            {
                becameGoose.Status = Status.Fox;
                becameGoose.Position = (Position)foxIndex;
                _playerRepo.UpdatePlayer(becameGoose.Name, becameGoose);
                humanPlayer.Status = Status.Duck;
                humanPlayer.Position = (Position)goosedPlayerIndex;
                _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                PrintPlayerPositions();
                Console.WriteLine($"You outran {becameGoose.Name} and took their spot!");
                Console.WriteLine(" ");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else
            {
                PrintPlayerPositions();
                Console.WriteLine($"{becameGoose.Name} caught you! They sit back down in their spot.");
                Console.WriteLine(" ");
                Console.WriteLine("Press enter to continue..");
                Console.ReadLine();
            }
        }


        public void BeFowl(Player humanPlayer)
        {
            PrintPlayerPositions();
            Player fox = _playerRepo.GetTheFox();
            Console.WriteLine($"{fox.Name} is the fox...");
            System.Threading.Thread.Sleep(2000);

            bool keepDuckLoopRunning = true;
            int duckCount = 0;
            int duckCountMax = RandomDuckNumberGenerator.GenerateRandomDuckNumber();
            while (keepDuckLoopRunning)
            {
                if (duckCount < duckCountMax)
                {
                    duckCount = duckCount + 1;
                    int targetIndex = (int)fox.Position;
                    if (targetIndex >= 11 && targetIndex <= 17) //11-18 are the indexes for fox in Position enum
                    {
                        targetIndex = targetIndex + 1;
                        fox.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        PrintPlayerPositions();
                        PrintDuckDeclarations(duckCount);
                    }
                    else if (targetIndex == 18) 
                    {
                        targetIndex = 11;
                        fox.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        PrintPlayerPositions();
                        PrintDuckDeclarations(duckCount);
                    }
                    System.Threading.Thread.Sleep(1300);
                }
                else
                {
                    int targetIndex = (int)fox.Position;
                    if (targetIndex >= 11 && targetIndex <= 17) //11-18 are the indexes for fox in Position enum
                    {
                        targetIndex = targetIndex + 1;
                        fox.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                    else if (targetIndex == 18)
                    {
                        targetIndex = 11;
                        fox.Position = (Position)targetIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        System.Threading.Thread.Sleep(500);
                        break;
                    }
                }
            }

            PrintPlayerPositions();
            int foxIndex = (int)fox.Position;
            int goosedPlayerIndex = foxIndex - 10;
            Player becameGoose = _playerRepo.GetPlayerByIndex(goosedPlayerIndex); 
            Console.WriteLine("GOOSE!!!");

            if (becameGoose == humanPlayer)
            {
                Console.WriteLine(" ");
                Console.WriteLine($"{humanPlayer.Name}!!! You're the goose!!!\n" +
                     $"Mash the spacebar to chase {fox.Name} around the circle before they take your spot!");
                Console.ReadKey();

                int spacePresses = ChaseSequence(humanPlayer);
                if (spacePresses >= (fox.Speed + 5)) //give a 5-step headstart to whoever is the fox
                {
                    humanPlayer.FoxesCaught = humanPlayer.FoxesCaught + 1;
                    _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                    if (humanPlayer.FoxesCaught >= 3)
                    {
                        humanPlayer.Status = Status.Winner;
                        _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                    }

                    PrintPlayerPositions();
                    Console.WriteLine($"You caught {fox.Name} and get to keep your spot in the circle!");
                    Console.WriteLine(" ");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    fox.Status = Status.Duck;
                    fox.Position = (Position)goosedPlayerIndex;
                    _playerRepo.UpdatePlayer(fox.Name, fox);
                    humanPlayer.Status = Status.Fox;
                    humanPlayer.Position = (Position)foxIndex;
                    _playerRepo.UpdatePlayer(humanPlayer.Name, humanPlayer);
                    PrintPlayerPositions();
                    Console.WriteLine($"You couldn't catch {fox.Name}!  They took your seat.");
                    Console.WriteLine(" ");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine(" ");
                Console.WriteLine($"{becameGoose.Name} is the goose! {becameGoose.Name} is chasing {fox.Name}..."); //becameGoose was null
                System.Threading.Thread.Sleep(3000);

                ChaseSequence(becameGoose);
                if (becameGoose.Speed >= (fox.Speed + 5)) //give a 5-step headstart to whoever is the fox
                {
                    PrintPlayerPositions();
                    Console.WriteLine($"{becameGoose.Name} caught {fox.Name} and keeps their spot in the circle.");
                    System.Threading.Thread.Sleep(3000);
                }
                else
                {
                    fox.Status = Status.Duck;
                    fox.Position = (Position)goosedPlayerIndex;
                    _playerRepo.UpdatePlayer(fox.Name, fox);
                    becameGoose.Status = Status.Fox;
                    becameGoose.Position = (Position)foxIndex;
                    _playerRepo.UpdatePlayer(becameGoose.Name, becameGoose);
                    PrintPlayerPositions();
                    Console.WriteLine($"{becameGoose.Name} couldn't catch {fox.Name}!\n" +
                        $"{fox.Name} sat down in {becameGoose.Name}'s spot.");
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }


        public void PrintPlayerPositions()
        {
            List<Player> playerList = _playerRepo.GetPlayers();
            string d1 = "  ";
            string d2 = "  ";
            string d3 = "  ";
            string d4 = "  ";
            string d5 = "  ";
            string d6 = "  ";
            string d7 = "  ";
            string d8 = "  ";
            string f1 = "  ";
            string f2 = "  ";
            string f3 = "  ";
            string f4 = "  ";
            string f5 = "  ";
            string f6 = "  ";
            string f7 = "  ";
            string f8 = "  ";
            string g1 = "  ";
            string g2 = "  ";
            string g3 = "  ";
            string g4 = "  ";
            string g5 = "  ";
            string g6 = "  ";
            string g7 = "  ";
            string g8 = "  ";

            foreach (Player player in playerList)
            {
                if (player.Position == Position.DuckOne)
                {
                    d1 = player.Avatar;
                }
                if (player.Position == Position.DuckTwo)
                {
                    d2 = player.Avatar;
                }
                if (player.Position == Position.DuckThree)
                {
                    d3 = player.Avatar;
                }
                if (player.Position == Position.DuckFour)
                {
                    d4 = player.Avatar;
                }
                if (player.Position == Position.DuckFive)
                {
                    d5 = player.Avatar;
                }
                if (player.Position == Position.DuckSix)
                {
                    d6 = player.Avatar;
                }
                if (player.Position == Position.DuckSeven)
                {
                    d7 = player.Avatar;
                }
                if (player.Position == Position.DuckEight)
                {
                    d8 = player.Avatar;
                }
                if (player.Position == Position.FoxOne)
                {
                    f1 = player.Avatar;
                }
                if (player.Position == Position.FoxTwo)
                {
                    f2 = player.Avatar;
                }
                if (player.Position == Position.FoxThree)
                {
                    f3 = player.Avatar;
                }
                if (player.Position == Position.FoxFour)
                {
                    f4 = player.Avatar;
                }
                if (player.Position == Position.FoxFive)
                {
                    f5 = player.Avatar;
                }
                if (player.Position == Position.FoxSix)
                {
                    f6 = player.Avatar;
                }
                if (player.Position == Position.FoxSeven)
                {
                    f7 = player.Avatar;
                }
                if (player.Position == Position.FoxEight)
                {
                    f8 = player.Avatar;
                }
                if (player.Position == Position.GooseOne)
                {
                    g1 = player.Avatar;
                }
                if (player.Position == Position.GooseTwo)
                {
                    g2 = player.Avatar;
                }
                if (player.Position == Position.GooseThree)
                {
                    g3 = player.Avatar;
                }
                if (player.Position == Position.GooseFour)
                {
                    g4 = player.Avatar;
                }
                if (player.Position == Position.GooseFive)
                {
                    g5 = player.Avatar;
                }
                if (player.Position == Position.GooseSix)
                {
                    g6 = player.Avatar;
                }
                if (player.Position == Position.GooseSeven)
                {
                    g7 = player.Avatar;
                }
                if (player.Position == Position.GooseEight)
                {
                    g8 = player.Avatar;
                }
            }
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(

$@"                 {g2}{f1}      {f2}{g3}
                   {d1}      {d2}

          {g1}                       {g4}
          {f8} {d8}                 {d3} {f3}


          {f7} {d7}                 {d4} {f4}
          {g8}                       {g5}

                   {d6}      {d5}
                 {g7}{f6}      {f5}{g6}");

        }


        public void PrintDuckDeclarations(int duckCount)
        {
            for (int i = 1; i <= duckCount; i++)
            {
                Console.WriteLine("Duck...");
            }
        }


        public int ChaseSequence(Player goose)
        {
            Player humanPlayer = _playerRepo.GetHumanPlayer();
            Player fox = _playerRepo.GetTheFox();

            int spacePresses = 0;
            if ((goose == humanPlayer) || (fox == humanPlayer))
            {
                //mashing sequence for if human is controlling fox or goose
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < 5000)
                {
                    ConsoleKeyInfo foxRunningInput = Console.ReadKey();
                    if (foxRunningInput.KeyChar == ' ')
                    {
                        spacePresses = spacePresses + 1;
                    }
                }
                stopwatch.Stop();

                //animation sequence is same no matter the status of human
                int foxOriginalIndex = (int)fox.Position;
                int foxIndex = foxOriginalIndex;
                int gooseOriginalIndex = (int)goose.Position;
                int gooseIndex = foxIndex + 10;

                for (int i = 1; i <= 7; i++)
                {
                    if (foxIndex >= 11 && foxIndex <= 18)
                    {
                        foxIndex = foxIndex + 1;
                        fox.Position = (Position)foxIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        gooseIndex = gooseIndex + 1;
                        goose.Position = (Position)gooseIndex;
                        _playerRepo.UpdatePlayer(goose.Name, goose);
                    }
                    if (foxIndex > 18)
                    {
                        foxIndex = 11;
                        fox.Position = (Position)foxIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        gooseIndex = 21;
                        goose.Position = (Position)gooseIndex;
                        _playerRepo.UpdatePlayer(goose.Name, goose);
                    }
                    Console.Clear();
                    PrintPlayerPositions();
                    System.Threading.Thread.Sleep(700);
                }

                fox.Position = (Position)foxOriginalIndex;
                _playerRepo.UpdatePlayer(fox.Name, fox);
                goose.Position = (Position)gooseOriginalIndex;
                _playerRepo.UpdatePlayer(goose.Name, goose);

                return spacePresses;
            }
            else
            {
                //animation sequence is same no matter the status of human
                int foxOriginalIndex = (int)fox.Position;
                int foxIndex = foxOriginalIndex;
                int gooseOriginalIndex = (int)goose.Position;
                int gooseIndex = foxIndex + 10;

                for (int i = 1; i <= 7; i++)
                {
                    if (foxIndex >= 11 && foxIndex <= 18)
                    {
                        foxIndex = foxIndex + 1;
                        fox.Position = (Position)foxIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        gooseIndex = gooseIndex + 1;
                        goose.Position = (Position)gooseIndex;
                        _playerRepo.UpdatePlayer(goose.Name, goose);
                    }
                    if (foxIndex > 18)
                    {
                        foxIndex = 11;
                        fox.Position = (Position)foxIndex;
                        _playerRepo.UpdatePlayer(fox.Name, fox);
                        gooseIndex = 21;
                        goose.Position = (Position)gooseIndex;
                        _playerRepo.UpdatePlayer(goose.Name, goose);
                    }
                    Console.Clear();
                    PrintPlayerPositions();
                    System.Threading.Thread.Sleep(700);
                }

                fox.Position = (Position)foxOriginalIndex;
                _playerRepo.UpdatePlayer(fox.Name, fox);
                goose.Position = (Position)gooseOriginalIndex;
                _playerRepo.UpdatePlayer(goose.Name, goose);

                return 0;
            }
        }


        private void GameIntro()
        {
            //TITLE SCREEN
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("                              DUCK DUCK GOOSE");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(@"                            ,----,
                       ___.`      `,
                       `===  D     :
                         `'.      .'
                            )    (                   ,
                           /      \_________________/|
                          /                          |
                         |                           ;
                         |               _____       /
                         |      \       ______7    ,'
                         |       \    ______7     /
                          \       `-,____7      ,'   
                    ^~^~^~^`\                  /~^~^~^~^
                      ~^~^~^ `----------------' ~^~^~^
                     ~^~^~^~^~^^~^~^~^~^~^~^~^~^~^~^~");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("                       ---Press Any Key to Start---");
            Console.ReadKey();

            //CREATE PLAYER SCREEN
            Console.Clear();
            Player player = new Player();
            Console.WriteLine("Enter a name for your player:");
            player.Name = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Choose an avatar for your player:\n" +
                "1: {}        2: oo        3: )(        4: ##");
            string avatarChoice = Console.ReadLine();
            switch (avatarChoice)
            {
                case "1":
                    player.Avatar = "{}";
                    break;
                case "2":
                    player.Avatar = "oo";
                    break;
                case "3":
                    player.Avatar = ")(";
                    break;
                case "4":
                    player.Avatar = "##";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose a number 1-4.");
                    break;
            }
            player.Status = Status.Fox;
            player.Position = Position.FoxOne;
            player.IsHumanPlayer = true;
            player.FoxesCaught = 0;
            _playerRepo.AddPlayer(player);
        }


        public void SeedComputerPlayers()
        {
            Player michaelBluth = new Player("Michael Bluth", "MB", Status.Duck, Position.DuckOne, false);
            Player lindsayFunke = new Player("Lindsay Funke", "LF", Status.Duck, Position.DuckTwo, false);
            Player gobBluth = new Player("Gob Bluth", "gB", Status.Duck, Position.DuckThree, false);
            Player busterBluth = new Player("Buster Bluth", "BB", Status.Duck, Position.DuckFour, false);
            Player lucilleBluth = new Player("Lucille Bluth", "LB", Status.Duck, Position.DuckSix, false);
            Player georgeBluth = new Player("George Bluth", "GB", Status.Duck, Position.DuckFive, false);
            Player tobiasFunke = new Player("Tobias Funke", "TF", Status.Duck, Position.DuckSeven, false);
            Player maebyFunke = new Player("Maeby Funke", "MF", Status.Duck, Position.DuckEight, false);

            _playerRepo.AddPlayer(michaelBluth);
            _playerRepo.AddPlayer(lindsayFunke);
            _playerRepo.AddPlayer(gobBluth);
            _playerRepo.AddPlayer(busterBluth);
            _playerRepo.AddPlayer(lucilleBluth);
            _playerRepo.AddPlayer(georgeBluth);
            _playerRepo.AddPlayer(tobiasFunke);
            _playerRepo.AddPlayer(maebyFunke);
        }
    }
}
