using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class ScolipedeMegaStoneItem : MegaStoneItem
	{
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MegaShardItem>(25)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofMight, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
