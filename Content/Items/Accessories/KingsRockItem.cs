using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Pokemod.Content.Items.EvoStones;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
	public class KingsRockItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.maxStack = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += 0.35f;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PearlstoneBlock, 50)
				.AddIngredient(ItemID.Seashell, 5)
				.AddIngredient(ItemID.GoldCrown, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.PearlstoneBlock, 50)
                .AddIngredient(ItemID.Seashell, 5)
                .AddIngredient(ItemID.PlatinumCrown, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}