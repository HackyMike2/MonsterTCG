using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    class SpellCard : Card
    {
        public SpellCard(string name, float damage, ListMaster.Type type) : base(name, damage, type)
        {

        }

        public override float CalculateDamage(Card opponent)
        {
            Console.WriteLine("Spell");
            return Damage;
        }
    }
}
