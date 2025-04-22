using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;
using Pokemod.Common.Systems;

namespace Pokemod.Content.Items.Pokeballs
{
	public class LevelballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<LevelballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<BlackApricorn>(), 1)
				.AddIngredient(ItemID.Ruby, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class LevelballProj : BallProj{
		public override bool FailureProb(float catchRate){
			float compareLvl = WorldLevel.MaxWorldLevel-5;
			float pokemonLvl = targetPokemon.GetGlobalNPC<PokemonNPCData>().lvl;

			if(pokemonLvl >= compareLvl) catchRate *= 1f;
			else if(pokemonLvl >= compareLvl/2) catchRate *= 2f;
			else if(pokemonLvl >= compareLvl/4) catchRate *= 4f;
			else catchRate *= 8f;

			return RegularProb(catchRate);
		}
	}
}
