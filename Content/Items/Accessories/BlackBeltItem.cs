using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Items.Accessories
{
	public class BlackBeltItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.maxStack = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += 0.15f;
		}
		
        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 50)
				.AddIngredient(ItemID.BlackDye, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
    }
}