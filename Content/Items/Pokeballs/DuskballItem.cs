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

namespace Pokemod.Content.Items.Pokeballs
{
	public class DuskballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<DuskballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeEmeraldSeed, 2)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class DuskballProj : BallProj{
		public override bool FailureProb(float catchRate){
			if(!Main.dayTime) catchRate *= 3f;

			return RegularProb(catchRate);
		}
	}
}
