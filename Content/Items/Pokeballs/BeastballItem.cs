using Pokemod.Content.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class BeastballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<BeastballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 0.1f;
		public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ItemID.GemTreeSapphireSeed, 5)
				.AddIngredient(ItemID.GemTreeAmberSeed, 5)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class BeastballProj : BallProj{
		public override bool FailureProb(float catchRate){
			if(targetPokemon.GetGlobalNPC<PokemonNPCData>().ultrabeast) catchRate = 5f;

			return RegularProb(catchRate);
		}
	}
}
