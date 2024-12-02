using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    public class MonsterCard : Card
    {
        public MonsterCard(string name, float damage, ListMaster.Type type) : base (name, damage, type) { }





        public override float CalculateDamage(Card opponent)
        {
           // if(Name.Contains("")) //auch nicht so geiler ansatz
           return Damage; /*Damage calkulieren*/
        }
    }


}