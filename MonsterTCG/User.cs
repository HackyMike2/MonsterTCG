using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MonsterTCG
{
    public class User
    {
        //public int Id { get; set; }
        public string UserName { get; set; }
        public int Id { get; set;}
        public string Password { get; set; }
        public int Coins { get; set; } = 20;
        public int Elo { get; set; } = 100;
        public string SecurityToken { get; set; }
        public List<Card> Deck { get; set; } // subTable
        public List<Card> Collection { get; set; } //subTable


        public User(string name, string PW) //for register
        {
            Random rnd = new Random();
            Id = 0;
            UserName = name;
            Password = PW; //plain text for now, hashed later. (i wish i had time for this)
            SecurityToken = GenerateToken();
            Coins = 20;
            Elo = 100;
            Deck = new List<Card>();

        }

        public User(int id,string name, string PW, int coins, int elo, string securitytoken) //for saving data from DB;
        {
            Id = id;
            UserName = name;
            Password = PW;
            Coins = coins;
            Elo = elo;
            SecurityToken = securitytoken;
        }

        public void AddCoins(int amount) { Coins += amount; } //just for winning or loosing coins

        public void UpdateElo(int amount)
        { if (!(Elo + amount < 0)) { Elo += amount; } } // can also be negative, dont go under 0!

        public string GenerateToken()
        {
            //Console.WriteLine("here is the Token");
            string token = UserName + "-mtcgToken";
            return token;
        }

        public bool CompareToken(string token1) { if (token1 == SecurityToken) { return true; } else { return false; }; } //if the User wants to do anything, and we need to check if he really is who he says he is.

        public void buyPack()
        {
            if (CompareToken("")) //for later
            {
                if (Coins >= ListMaster.Pack1_price)
                {
                    Coins -= ListMaster.Pack1_price;
                    this.Collection.Add(ListMaster.getRandomCard());
                }
            }
        }

    }
}
