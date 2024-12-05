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
            Console.WriteLine("Welcome to the Start of this Programm!");

            Card myMonsterCard = ListMaster.getRandomCard();
            Console.Write("Damage: "); Console.WriteLine(myMonsterCard.Damage);
            Console.Write("Type: "); Console.WriteLine(myMonsterCard.Type);
            Console.Write("Name: "); Console.WriteLine(myMonsterCard.Name);
            myMonsterCard.CalculateDamage(myMonsterCard);
            if(myMonsterCard is MonsterCard) { Console.WriteLine("this is a monsterCard!"); } //Works!
            else if(myMonsterCard is SpellCard) { Console.WriteLine("this is a spellCard!"); }
            else { Console.WriteLine("Something went wrong!"); }
            //Console.WriteLine(myMonsterCard.GetType()); //MonsterTCG.MonsterCard mymonster Card is Card, is typeof MonsterCard and is MonsterCard, NOT Type of Card

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


            Console.WriteLine("Now Server start:");
            HTTP Server = new HTTP();
            Server.startServer();
            Console.ReadKey();
        }

    }
}
