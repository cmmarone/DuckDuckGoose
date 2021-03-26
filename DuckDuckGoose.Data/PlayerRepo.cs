using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckGoose.Data
{
    public class PlayerRepo
    {
        protected readonly List<Player> _playerDirectory = new List<Player>();

        public bool AddPlayer(Player player)
        {
            int listSizeBefore = _playerDirectory.Count;
            _playerDirectory.Add(player);
            bool addSucceeded = (_playerDirectory.Count > listSizeBefore) ? true : false;
            return addSucceeded;
        }

        public List<Player> GetPlayers()
        {
            return _playerDirectory;
        }

        public bool UpdatePlayer(string playerName, Player updatedPlayer)
        {
            Player player = GetPlayerByName(playerName);

            if (player != null)
            {
                //once the console runs, our players don't change names, avatars, speed, or IsHuman
                //these props are all that will ever change during a game
                player.Status = updatedPlayer.Status;
                player.Position = updatedPlayer.Position;
                player.FoxesCaught = updatedPlayer.FoxesCaught;
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool DeletePlayer(Player player)
        //{
        //    int listSizeBefore = _playerDirectory.Count;
        //    _playerDirectory.Remove(player);
        //    bool deleteSucceeded = (_playerDirectory.Count < listSizeBefore) ? true : false;
        //    return deleteSucceeded;
        //}

        public Player GetPlayerByName(string playerName)
        {
            foreach (Player player in _playerDirectory)
            {
                if (player.Name.ToLower() == playerName.ToLower())
                {
                    return player;
                }
            }
            return null;
        }

        public Player GetPlayerByIndex(int index)
        {
            foreach (Player player in _playerDirectory)
            {
                if (player.Position == (Position)index)
                {
                    return player;
                }
            }
            return null;
    
        }

        public Player GetHumanPlayer()
        {
            foreach (Player player in _playerDirectory)
            {
                if (player.IsHumanPlayer == true)
                {
                    return player;
                }
            }
            return null;
        }

        public Player GetTheFox()
        {
            foreach (Player player in _playerDirectory)
            {
                if (player.Status == Status.Fox)
                {
                    return player;
                }
            }
            return null;
        }
    }
}
