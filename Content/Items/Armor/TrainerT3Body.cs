using Pokemod.Content.Items.EvoStones;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.DamageClasses;
using Pokemod.Common.Players;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class TrainerT3Body : ModItem
	{
		public static readonly int MaxPokemonLevelCap = 40;
		public static readonly int AdditivePokemonCritBonus = 7;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxPokemonLevelCap, AdditivePokemonCritBonus);

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 15; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{
			if (player.GetModPlayer<PokemonPlayer>().levelCap < MaxPokemonLevelCap) player.GetModPlayer<PokemonPlayer>().levelCap = MaxPokemonLevelCap;
			player.GetCritChance<PokemonDamageClass>() += AdditivePokemonCritBonus;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.JungleSpores, 10)
				.AddIngredient(ItemID.MeteoriteBar, 5)
				.AddIngredient<TrainerT2Body>(1)
				.AddRecipeGroup("Tier3GymBadges", 1)
				.AddTile(TileID.Anvils)
				.Register();
        }
	}
}
