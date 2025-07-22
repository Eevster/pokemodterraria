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

		public override void Unload()
		{
			GymBadgesT1 = null;
			GymBadgesT2 = null;
			GymBadgesT3 = null;
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
				ModContent.ItemType<CascadeBadge>());
			RecipeGroup.RegisterGroup("Tier3GymBadges", GymBadgesT3);
        }
    }
}