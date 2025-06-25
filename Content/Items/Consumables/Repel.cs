using Pokemod.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Consumables
{
    public class Repel : ModItem
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
            Item.buffTime = 4 * 60 * 60;
            Item.value = Item.buyPrice(silver: 1);
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ApricornSeed>(5)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddTile(TileID.Bottles) // Making this recipe be crafted at bottles will automatically make Alchemy Table's effect apply to its ingredients.
                .Register();
        }
    }
}