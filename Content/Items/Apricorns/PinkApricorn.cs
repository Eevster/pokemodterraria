using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Pokemod.Content.Items.Apricorns
{
    public class PinkApricorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = false;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 12;
            Item.height = 14;
            Item.value = 80;
        }


    }
}
