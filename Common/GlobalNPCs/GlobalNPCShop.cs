using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.EvoStones;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.GlobalNPCs
{
    public class GlobalNPCShop : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop) {
            if (shop.NpcType == NPCID.Merchant){
                shop.Add<Everstone>();
                shop.Add<ShinyCharm>();
            }
            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add<LinkingCordItem>();
            }
        }
    }
}