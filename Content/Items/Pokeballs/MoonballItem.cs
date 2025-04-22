using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;
using Pokemod.Common.Systems;

namespace Pokemod.Content.Items.Pokeballs
{
	public class MoonballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<MoonballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<BlackApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class MoonballProj : BallProj{
		public override bool FailureProb(float catchRate){
			string pokemonName = targetPokemon.GetGlobalNPC<PokemonNPCData>().pokemonName;

			if(pokemonName == "Nidorina" || pokemonName == "Nidorino" || pokemonName == "Clefairy" || pokemonName == "Jigglypuff" || pokemonName == "Skitty" || pokemonName == "Munna") catchRate *= 4f;

			return RegularProb(catchRate);
		}
	}
}
