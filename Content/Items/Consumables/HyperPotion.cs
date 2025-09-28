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
    public class HyperPotion : PokemonConsumableItem{
        int healAmount = 120 * 5;
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

        public override bool OnItemUse(Projectile proj){
			PokemonPetProjectile pokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			if(pokemonProj.currentHp > 0 && pokemonProj.currentHp < pokemonProj.finalStats?[0]){
                pokemonProj.regenHP(healAmount);
                Item.consumable = true;
                return true;
            }
            Item.consumable = false;
            return false;
		}

        public override bool OnItemInvUse(CaughtPokemonItem item, Player player){
            if(item.proj != null){
                if(item.proj.active){
                    if(item.proj.ModProjectile is PokemonPetProjectile proj){
                        if(proj.currentHp > 0 && proj.currentHp < proj.finalStats[0]){
                            proj.regenHP(healAmount);
                            ReduceStack(player, Item.type);
                            return true;
                        }
                    }
                }
            }
            if(item.currentHP > 0 && item.currentHP < item.GetPokemonStats()[0]){
                item.currentHP += healAmount;
                if(item.currentHP > item.GetPokemonStats()[0]){
                    item.currentHP = item.GetPokemonStats()[0];
                }
                ReduceStack(player, Item.type);
                return true;
            }

            return false;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PixieDust, 1)
                .AddIngredient(ItemID.HealingPotion, 1)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
}