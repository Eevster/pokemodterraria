using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Items.Collectables
{
    internal class HeartScale : ModItem
    {
        public override void SetDefaults()
        {
            Item.ResearchUnlockCount = 10;
            Item.width = 24; // The item texture's width
            Item.height = 24; // The item texture's height

            Item.maxStack = 99; // The item's max stack value
            Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
        }
    }
}
