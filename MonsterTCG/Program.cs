using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DBConnector db = new DBConnector();
            Console.WriteLine("Welcome to the Start of this Programm!");
            #region debug 
            //Console.WriteLine("Test");
            /*Card myMonsterCard = ListMaster.getRandomCard();
            Console.Write("Damage: "); Console.WriteLine(myMonsterCard.Damage);
            Console.Write("Type: "); Console.WriteLine(myMonsterCard.Type);
            Console.Write("Name: "); Console.WriteLine(myMonsterCard.Name);
            myMonsterCard.CalculateDamage(myMonsterCard);
            if(myMonsterCard is MonsterCard) { Console.WriteLine("this is a monsterCard!"); } //Works!
            else if(myMonsterCard is SpellCard) { Console.WriteLine("this is a spellCard!"); }
            else { Console.WriteLine("Something went wrong!"); }

            Console.WriteLine("now user");
            User myUser = new User("michael","pw");
            myUser.AddCoins(20);
            Console.WriteLine(myUser.Coins);
            Console.WriteLine(myUser.Collection[2].Name);
            Console.WriteLine(myUser.Collection[1].Name);
            Console.WriteLine(myUser.Collection.Count);
            myUser.buyPack();
            Console.WriteLine(myUser.Coins);
            Console.WriteLine(myUser.Collection.Count);
            */
            #endregion //is commented out
            #region debug2-DB Connections
            //db.DBAddCardToPlayerCollection("kienboec", 9);
            //db.DBTestConnection();
            //int CardID = db.DBGetIDFromRandomCard();
            //Console.WriteLine("The Random Card ID is:{0}", CardID);
            //User benutzer = new User("something", "something");

            //db.DBAddCardToPlayerCollection(benutzer.UserName, CardID);

            //int cards = db.DBgetCardCount(); //Works
            /*bool test = db.DBFindUser(benutzer.UserName);
            if (test)
            { Console.WriteLine("The User already exists"); } else 
            { Console.WriteLine("User Does not Exist");
                
                db.DBCreateUser(benutzer);
                Console.WriteLine("User Created");
            }*/
            //Console.WriteLine("TestLogin");
            //db.DBLoginUser(benutzer);
            //string name = "something";
            //User user = db.getFullUser(name);
            //string answer = db.getAllPacks();
            //Console.WriteLine(answer);
            //Console.WriteLine("Those are the Userdatas\nID:{0},Username:{1},PW:{2},Coins:{3},Elo:{4},Token:{5}", user.Id, user.UserName, user.Password, user.Coins.ToString(),user.Elo.ToString(),user.SecurityToken);
            #endregion
            Console.WriteLine("Starting HTTP server...");
            HTTP Server = new HTTP();
            Server.StartServer();
            
            while (true)
            {
                // do nothing, so that the server does not shut down.
            }
        }

    }
}
