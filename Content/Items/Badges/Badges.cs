
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.NPCs;
using System.Linq;
using Terraria.ModLoader;
using Pokemod.Common.UI.MoveLearnUI;
using System.Collections.Generic;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace Pokemod.Content.Items.Badges
{
	public abstract class GymBadge : ModItem
	{
		public virtual int badgeTier => 1;
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(badgeTier);

		public override void SetDefaults()
		{
			Item.width = 26; // The item texture's width
			Item.height = 26; // The item texture's height

			Item.rare = badgeTier;

			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(silver: 10);
		}
	}

	public class BoulderBadge : GymBadge
	{
		public override int badgeTier => 1;
	}
	
	public class CascadeBadge : GymBadge
	{
        public override int badgeTier => 2;
	}
}
