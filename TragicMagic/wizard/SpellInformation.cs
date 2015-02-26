using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TragicMagic;

//@Author: J. Brown / DrMelon
//@Date: 14/02/15
//@Purpose: The SpellInformation class is used to define the properties of a spell,
//          like its name or how much damage it should deal on impact.
//          Spell entities themselves will be constructed using this information 
//          by the wizard, and so will be slightly different when cast by light/dark wizards.
//


namespace TragicMagic
{
    class SpellInformation
    {
        // Spell properties
        public String spellName;
        public String spellType;
        public float spellDamage;
        public float spellSpeed;
        
        // Simple constructor
        public SpellInformation(String name, String type, float damage, float speed)
        {
            spellName = name;
            spellType = type;
            spellDamage = damage;
            spellSpeed = speed;
        }



    }
}
