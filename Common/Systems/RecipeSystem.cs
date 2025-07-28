using Pokemod.Content.Items.Badges;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Common.Systems
{
	public class RecipeSystem : ModSystem
	{
		public static RecipeGroup GymBadgesT1;
		public static RecipeGroup GymBadgesT2;
		public static RecipeGroup GymBadgesT3;
		public static RecipeGroup GymBadgesT4;
		public static RecipeGroup GymBadgesT5;
		public static RecipeGroup GymBadgesT6;
		public static RecipeGroup GymBadgesT7;
		public static RecipeGroup GymBadgesT8;

		public override void Unload()
		{
			GymBadgesT1 = null;
			GymBadgesT2 = null;
			GymBadgesT3 = null;
			GymBadgesT4 = null;
			GymBadgesT5 = null;
			GymBadgesT6 = null;
			GymBadgesT7 = null;
			GymBadgesT8 = null;
		}

		public override void AddRecipeGroups()
		{
			GymBadgesT1 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(1).Value}",
				ModContent.ItemType<BoulderBadge>());
			RecipeGroup.RegisterGroup("Tier1GymBadges", GymBadgesT1);

			GymBadgesT2 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(2).Value}",
				ModContent.ItemType<CascadeBadge>());
			RecipeGroup.RegisterGroup("Tier2GymBadges", GymBadgesT2);

			GymBadgesT3 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(3).Value}",
				ModContent.ItemType<ThunderBadge>());
			RecipeGroup.RegisterGroup("Tier3GymBadges", GymBadgesT3);

			GymBadgesT4 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(4).Value}",
				ModContent.ItemType<RainbowBadge>());
			RecipeGroup.RegisterGroup("Tier4GymBadges", GymBadgesT4);

			GymBadgesT5 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(5).Value}",
				ModContent.ItemType<SoulBadge>());
			RecipeGroup.RegisterGroup("Tier5GymBadges", GymBadgesT5);

			GymBadgesT6 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(6).Value}",
				ModContent.ItemType<MarshBadge>());
			RecipeGroup.RegisterGroup("Tier6GymBadges", GymBadgesT6);

			GymBadgesT7 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(7).Value}",
				ModContent.ItemType<VolcanoBadge>());
			RecipeGroup.RegisterGroup("Tier7GymBadges", GymBadgesT7);

			GymBadgesT8 = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetText("Mods.Pokemod.CommonItemTooltip.BadgesTier").WithFormatArgs(8).Value}",
				ModContent.ItemType<EarthBadge>());
			RecipeGroup.RegisterGroup("Tier8GymBadges", GymBadgesT8);
        }
    }
}