using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.Apricorns;
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
	public class PokeballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<PokeballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

		public override void AddRecipes()
		{
			CreateRecipe(8)
				.AddIngredient(ItemID.GemTreeRubySeed, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();

            //CoffeeCanUpdate
            CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<RedApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.Wood, 2)
                .AddIngredient(ItemID.StoneBlock, 1)
                .AddTile(TileID.WorkBenches)
				.Register();


        }



    }

	public class PokeballProj : BallProj{}
}
