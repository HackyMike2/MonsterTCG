using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{

    public static class ListMaster
    {
        private static Random rnd = new Random();
        public static int Pack1_price = 20;

        public enum Type { Fire, Water, Normal }

        public enum Species { Goblin, Wizzard, Ork, Knight, WaterSpell, Kraken, FireElves, Dragon, spell } //should i put none as a value or just leave it empty?i think the none optinon is better.

        public static List<Card> AllCards = new List<Card> //to be done in db
        {
        new MonsterCard("name",34,Type.Fire,Species.spell), //debug feature
        new MonsterCard("Goblin",50,Type.Normal, Species.Goblin),
        new MonsterCard("Wizzard", 60,Type.Water,Species.Wizzard),
        new MonsterCard("Knight",40,Type.Normal,Species.Knight),
        new MonsterCard("Kraken",50,Type.Water,Species.Kraken),
        new MonsterCard("FireElves",40,Type.Fire,Species.FireElves),
        new MonsterCard("Dragon",60,Type.Fire,Species.Dragon),


        new SpellCard("WaterSpell",50,Type.Water),
        new SpellCard("FireSpell",50,Type.Fire),
        new SpellCard("Car",60,Type.Normal)
        };

        public static Card getRandomCard()
        {


            return AllCards[rnd.Next(AllCards.Count)];
            //HIER DANN STATDESSEN random aus der DB
        }


    }



}
