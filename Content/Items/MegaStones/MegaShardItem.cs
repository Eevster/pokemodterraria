
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class MegaShardItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;
			Item.material = true;
			Item.maxStack = Item.CommonMaxStack;
            Item.ResearchUnlockCount = 50;

            Item.value = Item.buyPrice(copper: 20);
		}
	}
}
