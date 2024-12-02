using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public int Coins { get; set; } = 20;
        //card deck, and card stack
        public int Elo { get; set; } = 100;
        public string SecurityToken { get; set; }


        public void AddCoins(int amount) {Coins += amount;}
        public void UpdateElo(int amount) 
        {if(!(Elo + amount < 0)) {  Elo += amount; }} // can also be negative, not go under 0
        public void GenerateToken() {/*generate token and send it to Security Token.*/ }

        public void CompareToken(string token1, string token2) { }



    }
}
