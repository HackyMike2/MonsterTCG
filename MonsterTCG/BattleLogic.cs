using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG //thank god Stack overflow exists.
{
    public class WaitingPlayers
    {
        public int Id { get; set; } // Player ID
    }

    public static class BattleLogic
    {
        private static ConcurrentQueue<int> waitingQueue = new ConcurrentQueue<int>();

        public static void AddPlayerToQueue(int playerId)
        {
            waitingQueue.Enqueue(playerId);
            Console.WriteLine($"Player {playerId} added to the queue.");
        }

        public static (int, int)? StartBattle()
        {
            if (waitingQueue.TryDequeue(out int player1) && waitingQueue.TryDequeue(out int player2))
            {
                Console.WriteLine($"Starting battle between Player {player1} and Player {player2}.");
                return (player1, player2);
            }

            Console.WriteLine("Not enough players for a battle.");
            return null;
        }


        public static string Fight(int id1, int id2) 
        {
            return "";
        }
    }
}
