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
using Terraria.Localization;

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

			attackType = player.GetModPlayer<PokemonPlayer>().attackMode;

			if (player.altFunctionUse == 2) {
				switch(attackType){
					case 0:
						attackType = 1;
						break;
					case 2:
						attackType = 0;
						break;
				}
				
                SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				player.GetModPlayer<PokemonPlayer>().ChangeAttackMode(attackType);
			}
			else {
				switch(attackType){
					case 0:
						attackType = 2;
						break;
					case 1:
						attackType = 0;
						break;
				}

				SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				player.GetModPlayer<PokemonPlayer>().ChangeAttackMode(attackType);
			}

            return true;
        }

        public override void UpdateInventory(Player player)
        {
            attackType = player.GetModPlayer<PokemonPlayer>().attackMode;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			foreach (TooltipLine line in tooltips) {
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
					if(attackType == 0){
						line.Text = Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltip").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.AutoAttack")).Value
						+ "\n" + Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltipLeft").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.DirectedAttack")).Value
						+ "\n" + Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltipRight").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.NoAttack")).Value;
					}
                    if(attackType == 1){
						line.Text = Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltip").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.NoAttack")).Value
						+ "\n" + Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltipLeft").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.AutoAttack")).Value;
					}
                    if(attackType == 2){
						line.Text = Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltip").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.DirectedAttack")).Value
						+ "\n" + Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltipLeft").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.DirectedAttack")).Value
						+ "\n" + Language.GetText("Mods.Pokemod.PokemonInfo.GloveTooltipRight").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonInfo.AutoAttack")).Value;
					}
				}
			}

            base.ModifyTooltips(tooltips);
		}
		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 50)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddTile(TileID.Anvils) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
	}
}