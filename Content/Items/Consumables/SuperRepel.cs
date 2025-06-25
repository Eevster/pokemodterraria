using Pokemod.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Consumables
{
    public class SuperRepel : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.UseSound = SoundID.Item13;
            Item.maxStack = Item.CommonMaxStack;
            Item.buffType = ModContent.BuffType<PokemonRepel>();
            Item.buffTime = 8 * 60 * 60;
            Item.value = Item.buyPrice(silver: 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Repel>(1)
                .AddIngredient(ItemID.VilePowder, 2)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
                
            CreateRecipe()
                .AddIngredient<Repel>(1)
                .AddIngredient(ItemID.ViciousPowder, 2)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
}