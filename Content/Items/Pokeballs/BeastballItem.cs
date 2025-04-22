using Pokemod.Content.NPCs;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class BeastballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<BeastballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 0.1f;
		public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 5)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 5)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class BeastballProj : BallProj{
		public override bool FailureProb(float catchRate){
			if(targetPokemon != null){
				if(targetPokemon.GetGlobalNPC<PokemonNPCData>().ultrabeast) catchRate = 5f;
			}

			return RegularProb(catchRate);
		}
	}
}
