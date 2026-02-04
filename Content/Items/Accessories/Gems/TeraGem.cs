using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;

namespace Pokemod.Content.Items.Accessories.Gems
{
    public class TeraGem : ModItem
	{
		public float damageMult = 0.1f;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(100f*damageMult));
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 5);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += damageMult;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<BugGem>(1)
				.AddIngredient<DarkGem>(1)
				.AddIngredient<DragonGem>(1)
				.AddIngredient<ElectricGem>(1)
				.AddIngredient<FairyGem>(1)
				.AddIngredient<FightingGem>(1)
				.AddIngredient<FireGem>(1)
				.AddIngredient<FlyingGem>(1)
				.AddIngredient<GhostGem>(1)
				.AddIngredient<GrassGem>(1)
				.AddIngredient<GroundGem>(1)
				.AddIngredient<IceGem>(1)
				.AddIngredient<NormalGem>(1)
				.AddIngredient<PoisonGem>(1)
				.AddIngredient<PsychicGem>(1)
				.AddIngredient<RockGem>(1)
				.AddIngredient<SteelGem>(1)
				.AddIngredient<WaterGem>(1)
				.AddIngredient(ItemID.SoulofLight, 9)
				.AddIngredient(ItemID.SoulofNight, 9)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}