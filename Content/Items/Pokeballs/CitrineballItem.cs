using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class CitrineballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<CitrineballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class CitrineballProj : BallProj{}
}
