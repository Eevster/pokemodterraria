using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;
using Pokemod.Common.Systems;

namespace Pokemod.Content.Items.Pokeballs
{
	public class NetballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<NetballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<GreenApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class NetballProj : BallProj{
		public override bool FailureProb(float catchRate){
			PokemonInfo pokeInfo = PokemonData.pokemonInfo[targetPokemon.GetGlobalNPC<PokemonNPCData>().pokemonName];

			if(pokeInfo.HasType(TypeIndex.Water) || pokeInfo.HasType(TypeIndex.Bug)) catchRate *= 3.5f;

			return RegularProb(catchRate);
		}
	}
}
