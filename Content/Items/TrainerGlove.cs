using Pokemod.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using System;
using Pokemod.Common.Players;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Pokemod.Content.Items
{
	public class TrainerGlove : ModItem
	{
        public int attackType = 0; // keeps track of which attack it is
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.width = 28; // The item texture's width
			Item.height = 28; // The item texture's height
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
			Item.maxStack = 1; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}
		
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

		public override bool? UseItem(Player player)
		{
            if (player.whoAmI != Main.myPlayer) {
				return true;
			}

			if (player.altFunctionUse == 2) {
				attackType = (attackType+1)%3;
                CombatText.NewText(player.Hitbox, new Color(255, 255, 255), player.GetModPlayer<PokemonPlayer>().GetAttackModeText(attackType));
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
			}
			else {
				player.GetModPlayer<PokemonPlayer>().ChangeAttackMode(attackType);
			}

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			foreach (TooltipLine line in tooltips) {
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
                    line.Text = "Left click to make your pokemon perform: ";
					if(attackType == 0) line.Text += "Auto Attack";
                    if(attackType == 1) line.Text += "No Attack";
                    if(attackType == 2) line.Text += "Directed Attack";
                    line.Text += "\nRight click to switch modes";
				}
			}

            base.ModifyTooltips(tooltips);
		}
		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 50)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
	}
}