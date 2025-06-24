using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Items.Accessories
{
	public class ExpertBeltItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.maxStack = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += 0.40f;
		}
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<BlackBeltItem>(), 5)
				.AddIngredient(ItemID.Obsidian, 50)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}