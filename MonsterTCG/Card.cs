using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG
{
    public abstract class Card
    {
        public Card(string name, float damage, ListMaster.Type cardType)
        {
            Name = name;
            Damage = damage;
            Type = cardType;
        }
        public string Name { get; set; }

        public float Damage { get; set; }

        public ListMaster.Type Type { get; set; }

        public abstract float CalculateDamage(Card opponent);


    }
}
