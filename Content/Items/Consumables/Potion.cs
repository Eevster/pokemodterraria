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
    public class Potion : PokemonConsumableItem{
        int healAmount = 20 * 5;
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
                .AddIngredient(ItemID.Mushroom, 1)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
    /*public class Potion : ModItem
    {

        public Projectile pokemonProj;
        public PokemonPetProjectile pokemonMainProj;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(59, 0, 148),
                new Color(135, 0, 199),
                new Color(152, 118, 204)
            };
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Item92;
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = Item.CommonMaxStack;
            Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(0, 5));
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Find the tooltip line that corresponds to 'Heals ... life'
            // See https://tmodloader.github.io/tModLoader/html/class_terraria_1_1_mod_loader_1_1_tooltip_line.html for a list of vanilla tooltip line names
            TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "HealLife");

            if (line != null)
            {
                // Change the text to 'Heals max/2 (max/4 when quick healing) life'
                line.Text = "Heal Pokemon for 20 HP";
            }
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].ModProjectile is PokemonPetProjectile)
                {
                    pokemonProj = Main.projectile[i];
                }
            }
            if (pokemonProj != null)
            {

                if (pokemonProj.ModProjectile is PokemonPetProjectile)
                {
                    pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;

                    if (pokemonMainProj.currentHp <= pokemonMainProj.finalStats[0])
                    {
                        return true;
                    }
                    Main.NewText("Your pokemon's health is full", 133, 255, 148);
                    return false;
                }
                return false;
            }
            Main.NewText("There's a time and place for everything, but not now.", 227, 89, 0);
            return false;
        }

        public override bool? UseItem(Player player)
        {
            pokemonMainProj.regenHP(20);
            return true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 1)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }*/
}