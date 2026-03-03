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
	public class BlueTrainerGlove : TrainerGlove
	{
		private readonly int ExtraDamage = 4;
		private readonly int GloveRange = 6;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Item.rare = ItemRarityID.Green;
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
                .AddIngredient(ItemID.FlinxFur, 7)
                .AddIngredient(ItemID.WaterCandle, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
	}
}