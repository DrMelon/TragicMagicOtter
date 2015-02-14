using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TragicMagic;

//@Author: J. Brown / DrMelon
//@Date: 14/02/15
//@Purpose: Provides a singleton class for looking up spells by combos.
//@Usage: Example usage is as follows:
/////////////////////////////////////////
//         SpellInformation whatSpellDidIJustCast = ComboSystem::Instance.CheckSpell(myInput);
//         Wizard.CastSpell(whatSpellDidIJustCast);
/////////////////////////////////////////
//
//      CheckSpell() will return null if no spell with that combo is found. 
//
//
//       When adding spells to the dictionary, bear the following in mind:
//
//       *  Combos are defined as strings of button inputs
//       *  The inputs correspond to elements: Fire, Water, Earth, Lightning
//       *  However, because Otter2D's input management is based on Xbox controls, the string will be made of A,B,X,Y inputs
//       *  An example combo would be "XXYA"
//       *  Elements are mapped to the relevant colour on an xbox controller
//       *  Fire: B (Red)
//       *  Earth: A (Green)
//       *  Water: X (Blue)
//       *  Lightning: Y (Yellow)
//
// 


namespace TragicMagic
{
    class ComboSystem
    {
        private static ComboSystem instance; // The internal instance of the combo system
        private Dictionary<String, SpellInformation> spellDictionary; // The lookup table of spells
       

        //Constructor is always private in singleton pattern.
        private ComboSystem() 
        {
            // Temporary spell definitions here;
            //TODO: Make these load from files or spell creator or something.

            // Magic number values because no balance/tweaking information available yet
            AddSpell("B", new SpellInformation("Fire I", 0.5f, 10.0f));
            AddSpell("A", new SpellInformation("Earth I", 0.5f, 10.0f));
            AddSpell("X", new SpellInformation("Water I", 0.5f, 10.0f));
            AddSpell("Y", new SpellInformation("Lightning I", 0.5f, 10.0f));

            AddSpell("XBAYAB", new SpellInformation("xX=FACE OF THE MAKER=Xx", 9999.0f, 10.0f));


        }

        // Allows on-the-fly spell additions.
        public void AddSpell(String comboAdd, SpellInformation newSpell)
        {
            spellDictionary.Add(comboAdd, newSpell);
        }

        //Look up a spell!
        public SpellInformation CheckSpell(String combo)
        {
            // If a spell with that combo is in the dictionary, return it.
            SpellInformation spellOut = null;
            if(spellDictionary.TryGetValue(combo, out spellOut))
            {
                return spellOut;
            }

            // Else return null.
            return null;
        }

        //The accessible instance of the combo system
        public static ComboSystem Instance 
        {
            get
            {
                if(instance == null)
                {
                    instance = new ComboSystem();
                }
                return instance;
            }
        }

        



    }
}
