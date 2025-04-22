using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;

namespace Pokemod.Content.Items.Pokeballs
{
	public class FastballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<FastballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 2)
				.AddIngredient(ModContent.ItemType<RedApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class FastballProj : BallProj{
		public override bool FailureProb(float catchRate){
			if(PokemonNPCData.pokemonInfo[targetPokemon.GetGlobalNPC<PokemonNPCData>().pokemonName].pokemonStats[5] >= 100) catchRate *= 4f;

			return RegularProb(catchRate);
		}
	}
}
