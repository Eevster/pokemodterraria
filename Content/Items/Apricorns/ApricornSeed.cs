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
    public class ApricornSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = false;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ApricornPlant>());
            Item.maxStack = Item.CommonMaxStack;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(copper: 2);
        }
    }
}
