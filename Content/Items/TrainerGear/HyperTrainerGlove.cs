using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Common.Players;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Localization;

namespace Pokemod.Content.Items.TrainerGear
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class HyperTrainerGlove : TrainerGlove
	{
		private readonly int ExtraDamage = 6;
		private readonly int GloveRange = 10;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
        }

        public override void HoldItem(Player player)
        {
			player.handon = EquipLoader.GetEquipSlot(Mod, Item.ModItem.Name, EquipType.HandsOn);
			player.GetModPlayer<PokemonPlayer>().trainerGloveExtraDamage += ExtraDamage;
			player.GetModPlayer<PokemonPlayer>().trainerGloveRange += GloveRange;
        }

        public override string SetInitialTooltip()
		{
			return Language.GetText($"Mods.Pokemod.Items.{Item.ModItem.Name}.Tooltip").WithFormatArgs(GloveRange, ExtraDamage).Value;
		}
		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 7)
				.AddIngredient(ItemID.Bone, 5)
                .AddIngredient(ItemID.GoldBar, 5)
                .AddTile(TileID.Anvils)
                .Register();

			CreateRecipe()
                .AddIngredient(ItemID.Silk, 7)
				.AddIngredient(ItemID.Bone, 5)
                .AddIngredient(ItemID.PlatinumBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
	}
}