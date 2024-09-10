using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.NPCs
{
    public class Pokemon : ModNPC
    {
        public String name,type1,type2;
        public int ID, CryID, baseHp, baseAtk, baseDef, baseSpdef, baseSpatk, baseSpeed;
        public NPCID movementType;
        public float aggro;
        

        public int StatFunc(bool HP,int baseStat,int IV,int EV, int Level)
        {
            int done = 0;
            if(HP) {done = ((2 * baseStat + IV + (EV/4) * Level)/100) + Level + 10;}
            if(!HP) { done = ((2 * baseStat + IV + (EV / 4) * Level) / 100) + 5;}
            return done;
        }
       
        public int GenerateIV()
        {
            return Main.rand.Next(32);
        }

       

    }
}
