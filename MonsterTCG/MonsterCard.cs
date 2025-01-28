using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    public class MonsterCard : Card
    {
        public ListMaster.Species Species { get; set; }

        public MonsterCard(string name, float damage, ListMaster.Type type, ListMaster.Species species) : base(name, damage, type)
        {
            Species = species;
        }




        public override float CalculateDamage(Card opponent) // NOT USED ANYMORE!
        {
            // if(Name.Contains("")) //kein guter ansatz. species wurde dafür hinzugefügt.
            //Console.WriteLine(Species.ToString()); //DEBUG!
            if (opponent is MonsterCard enemy) //Kampf gegen ein Monster
            {
                if (Species == ListMaster.Species.Goblin && enemy.Species == ListMaster.Species.Dragon) { return 0; }
                if (Species == ListMaster.Species.Ork && enemy.Species == ListMaster.Species.Wizzard) { return 0; }
                if (Species == ListMaster.Species.Dragon && enemy.Species == ListMaster.Species.FireElves) { return 0; }
                return Damage;
            }
            else //kampf monster gegen zauber
            {
                if ((Type == ListMaster.Type.Fire && opponent.Type == ListMaster.Type.Normal) ||
                    (Type == ListMaster.Type.Water && opponent.Type == ListMaster.Type.Fire) ||
                    (Type == ListMaster.Type.Normal && opponent.Type == ListMaster.Type.Water))
                {
                    Console.WriteLine("DoubleDamage!");
                    return Damage * 2;
                }
                else if ((Type == ListMaster.Type.Water && opponent.Type == ListMaster.Type.Normal) ||
                    (Type == ListMaster.Type.Normal && opponent.Type == ListMaster.Type.Fire) ||
                    (Type == ListMaster.Type.Fire && opponent.Type == ListMaster.Type.Water))
                {
                    return Damage / 2;
                }
                else { return Damage; }
            }
            // return Damage; /*Damage calkulieren*/ /*code unerreichbar*/
        }
    }


}