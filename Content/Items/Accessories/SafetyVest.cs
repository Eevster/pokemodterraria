using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class SafetyVest : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(silver: 80);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().statMult[2] += 0.15f;
			player.GetModPlayer<PokemonPlayer>().statMult[4] += 0.15f;
			player.GetModPlayer<PokemonPlayer>().HasEjectButton = 3;
			player.GetModPlayer<PokemonPlayer>().HasLuminousMoss = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<AssaultVestItem>(1)
				.AddIngredient<EjectButton>(1)
				.AddIngredient<LuminousMoss>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}