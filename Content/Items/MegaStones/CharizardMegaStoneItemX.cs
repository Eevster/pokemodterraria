using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class CharizardMegaStoneItemX : MegaStoneItem
	{
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MegaShardItem>(25)
                .AddIngredient(ItemID.SoulofSight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
