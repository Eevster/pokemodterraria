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
	public class HallowedTrainerGlove : TrainerGlove
	{
		private readonly int ExtraDamage = 45;
		private readonly int GloveRange = 15;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(GloveRange, ExtraDamage);

		public override void SetDefaults()
        {
            base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
        }

        public override void HoldItem(Player player)
        {
			player.handon = EquipLoader.GetEquipSlot(Mod, Item.ModItem.Name, EquipType.HandsOn);
			player.GetModPlayer<PokemonPlayer>().trainerGloveExtraDamage += ExtraDamage;
			player.GetModPlayer<PokemonPlayer>().trainerGloveRange += GloveRange;
        }

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 7)
				.AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
	}
}