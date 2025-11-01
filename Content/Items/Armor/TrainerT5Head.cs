using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Items.Armor;
using Pokemod.Content.DamageClasses;
using Pokemod.Common.Players;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class TrainerT5Head : ModItem
	{
		public static readonly int AdditivePokemonCritBonus = 10;
		public static readonly int MaxPokemonIncrease = 2;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AdditivePokemonCritBonus);
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;

			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(MaxPokemonIncrease);
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 12; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<TrainerT5Body>() && legs.type == ModContent.ItemType<TrainerT5Legs>();
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<PokemonDamageClass>() += AdditivePokemonCritBonus;
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.GetModPlayer<PokemonPlayer>().maxPokemon += MaxPokemonIncrease;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.CobaltBar, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.PalladiumBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
