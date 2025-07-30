using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria.Localization;
using Terraria.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
// 0: Hardy
// 1: Lonely
// 2: Adamant
// 3: Naughty
// 4: Brave
// 10: Bold
// 11: Docile
// 12: Impish
// 13: Lax
// 14: Relaxed
// 20: Modest
// 21: Mild
// 22: Bashful
// 23: Rash
// 24: Quiet
// 30: Calm
// 31: Gentle
// 32: Careful
// 33: Quirky
// 34: Sassy
// 40: Timid
// 41: Hasty
// 42: Jolly
// 43: Naive
// 44: Serious
namespace Pokemod.Content.Items.Consumables.Mints
{
    public class AdamantMint : PokemonConsumableItem{
        public override void SetDefaults() {
			Item.width = 24; // The item texture's width
			Item.height = 24; // The item texture's height

			Item.useTime = 1;
			Item.useAnimation = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item1;

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.

            Item.consumable = true;
		}

        public override bool OnItemInvUse(CaughtPokemonItem item, Player player){
            item.nature = 2;
            
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 20)
                .AddIngredient(ItemID.Diamond, 2)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
}