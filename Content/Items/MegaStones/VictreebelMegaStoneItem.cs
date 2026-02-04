using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class VictreebelMegaStoneItem : MegaStoneItem
	{
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MegaShardItem>(25)
                .AddIngredient(ItemID.SoulofMight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
