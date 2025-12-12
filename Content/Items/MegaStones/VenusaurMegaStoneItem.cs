using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class VenusaurMegaStoneItem : MegaStoneItem
	{
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<MegaShardItem>(25)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
