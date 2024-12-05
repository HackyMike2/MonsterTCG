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
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; } = 20;
        public int Elo { get; set; } = 100;
        public string SecurityToken { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> Collection { get; set; }


        public User(string name, string PW) //register
        {
            Random rnd = new Random();
            Id = rnd.Next(int.MaxValue); //temp, will be db_user_id
            UserName = name;
            PW = Password; //plain text for now, hashed later.
            SecurityToken = GenerateToken();
            Coins = 20;
            Elo = 100;
            //todo security token
            Deck = new List<Card>();
            Collection = new List<Card>();
            Collection.Add(ListMaster.getRandomCard());
            Collection.Add(ListMaster.getRandomCard());
            Collection.Add(ListMaster.getRandomCard());
            Collection.Add(ListMaster.getRandomCard());
            Deck = Collection; //gives 4 random cards and also asigns them as fighting cards

        }
        public void AddCoins(int amount) {Coins += amount;} //just for winning or loosing coins

        public void UpdateElo(int amount) 
        {if(!(Elo + amount < 0)) {  Elo += amount; }} // can also be negative, dont go under 0!

        public string GenerateToken()
        {
            Console.WriteLine("here is the Token");
            return Guid.NewGuid().ToString();
        }

        public bool CompareToken(string token1, string SecurityToken) { if (token1 == SecurityToken) { return true; } else { return false; }; } //if the User wants to do anything, and we need to check if he really is who he says he is.

        public void buyPack() 
        {
            if (CompareToken("", "")) //for later
            {
                if(Coins >= ListMaster.Pack1_price)
                {
                    Coins -= ListMaster.Pack1_price;
                    this.Collection.Add(ListMaster.getRandomCard());
                }
            }
        }

    }
}
