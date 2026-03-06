using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Items.Accessories
{
	public class AssaultVestItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
            Item.maxStack = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<PokemonPlayer>().statMult[2] += 0.2f;
			player.GetModPlayer<PokemonPlayer>().statMult[4] += 0.2f;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 25)
                .AddIngredient(ItemID.RedDye, 20)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 25)
                .AddIngredient(ItemID.RedDye, 20)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}