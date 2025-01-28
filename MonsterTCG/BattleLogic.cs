using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static MonsterTCG.ListMaster;

namespace MonsterTCG //thank god Stack overflow exists.
{
    /*public class WaitingPlayers
    {
        public int Id { get; set; } // Player ID
    }*/

    public static class BattleLogic
    {
        private static ConcurrentQueue<int> waitingQueue = new ConcurrentQueue<int>();

        public static void AddPlayerToQueue(int playerId)
        {
            waitingQueue.Enqueue(playerId);
            Console.WriteLine("Player {0} added to the queue.",playerId);
        }

        public static ConcurrentQueue<int> GetQueue() 
        {
            return waitingQueue;
        }

        public static void SetQueue() 
        {
            waitingQueue = new ConcurrentQueue<int>();
        }

        public static (int, int)? StartBattle()
        {
            var queueArray = waitingQueue.ToArray();
            if (waitingQueue.Count > 1 && queueArray[0] != queueArray[1])
            {
                if (waitingQueue.TryDequeue(out int player1) && waitingQueue.TryDequeue(out int player2)) //er dequed mir automatisch immer player 1...
                {
                    Console.WriteLine("Starting battle between Player {0} and Player {1}.", player1, player2);
                    return (player1, player2);
                }
            }
            Console.WriteLine("Not enough players for a battle.");
            return null;
        }

        public static (int, string) Fight(User user1, User user2, tempCard[] user1Cards, tempCard[] user2Cards) 
        {
            //Console.WriteLine("I am in Fight!"); //DEBUG!!! 
            string Log = ""; 
            int round = 0; 
            if(user1Cards.Length == 0 && user2Cards.Length == 0) { return (0, Log); }
            if(user1Cards.Length == 0) { return (2, Log); } 
            if (user2Cards.Length == 0) { return (1, Log); }
            //aufpassen dass kein null dabei ist!
            user1Cards = user1Cards.Where(card => card != null).ToArray();
            user2Cards = user2Cards.Where(card => card != null).ToArray();
            while (user1Cards.Length > 0 && user2Cards.Length > 0 && round < 50) 
            {
                round++; 
                Log += "Round: " + round + ":\n"; 
                tempCard user1FightCard = user1Cards[0]; 
                tempCard user2FightCard = user2Cards[0];
                int result = 0;
                int DamageCard1 = FightLogic(user1FightCard,user2FightCard);
                int DamageCard2 = FightLogic(user2FightCard, user1FightCard);
                if (DamageCard1 > DamageCard2) { result = 1; } else if (DamageCard2 > DamageCard1) { result = 2; } else { result = 0; }
                
                if(result == 1) //Player 1 won
                {
                    Log += "Player " + user1.UserName + " Won with " + user1FightCard.Name + " against Player " + user2.UserName + " with " + user2FightCard.Name + ":\n";
                    //Card logic (take away loosers card, put winners card in the back, give loosers card to winner
                    user1Cards = user1Cards.Skip(1).Append(user1FightCard).ToArray(); //card to the back
                    user1Cards = user1Cards.Append(user2FightCard).ToArray(); //winner takes card
                    user2Cards = user2Cards.Skip(1).ToArray(); //remove loosers card

                }
                else if(result == 2) //Player 2 won
                {
                    Log += "Player " + user2.UserName + " Won with " + user2FightCard.Name + " against Player " + user1.UserName + " with " + user1FightCard.Name + ":\n";
                    //Card logic (take away loosers card, put winners card in the back, give loosers card to winner
                    user2Cards = user2Cards.Skip(1).Append(user2FightCard).ToArray(); //card to the back
                    user2Cards = user2Cards.Append(user1FightCard).ToArray(); //winner takes card
                    user1Cards = user1Cards.Skip(1).ToArray(); //remove loosers card
                }
                else //draw 
                {
                    Log += "This Fight Was a Draw between " + user1.UserName + "'s " + user1FightCard.Name + " against Player " + user2FightCard.Name + "'s " + user2FightCard.Name + ":\n";
                    //Card logic( both cards to the back.)
                    user1Cards = user1Cards.Skip(1).Append(user1FightCard).ToArray();
                    user2Cards = user2Cards.Skip(1).Append(user2FightCard).ToArray();
                }

            }
            if (user1Cards.Length == 0) { Console.WriteLine("Player {0} Won!",user2.UserName); return (2, Log); }
            else if(user2Cards.Length == 0) { Console.WriteLine("Player {0} Won!",user1.UserName); return (1, Log); }
            else
            {
                Console.WriteLine("Draw!");
                return (0, Log); //draw
            }

        }

        public static int FightLogic(tempCard user1FightCard, tempCard user2FightCard) 
        {
            if (user1FightCard.Species != 8 && user2FightCard.Species != 8) //MonsterFight, element is egal, nur special schauen
            {
                if(user1FightCard.Species == 0 && user2FightCard.Species == 7 ) { return 0; }
                if(user1FightCard.Species == 2 && user2FightCard.Species == 1 ) { return 0; }
                if(user1FightCard.Species == 7 && user2FightCard.Species == 6 ) { return 0; }
                else {  return user1FightCard.Damage; }
            }
            else //spell vs monster or monster vs spell, nothing applies?
            {
                if(user1FightCard.Type == 0  && user2FightCard.Type == 2 || user1FightCard.Type == 1 && user2FightCard.Type == 0 || user1FightCard.Type == 2 && user2FightCard.Type == 1) 
                {
                    //add winner elo by 5, reduce looser by 3
                    return user1FightCard.Damage * 2;
                }
                else if (user1FightCard.Type == 1 && user2FightCard.Type == 2 || user1FightCard.Type == 2 && user2FightCard.Type == 0 || user1FightCard.Type == 0 && user2FightCard.Type == 1) 
                {
                    return user1FightCard.Damage / 2;
                }
                else {  return user1FightCard.Damage; }
            }
        }


    }
}
