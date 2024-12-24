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

namespace Pokemod.Content.Items.Consumables
{
    public class MaxRevive : PokemonConsumableItem{
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
            if(item.currentHP == 0){
                item.currentHP = item.GetPokemonStats()[0];
                ReduceStack(player, Item.type);
                return true;
            }
            return false;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.UnicornHorn, 5)
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
}