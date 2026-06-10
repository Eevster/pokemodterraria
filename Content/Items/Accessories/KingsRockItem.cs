using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
	public class KingsRockItem : PokemonConsumableItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.maxStack = 1;
			Item.accessory = true;

            Item.useTime = 1;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item1;
        }

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += 0.35f;
		}

        public override bool OnItemUse(Projectile proj)
        {
            PokemonPetProjectile pokemonProj = (PokemonPetProjectile)proj.ModProjectile;
            if (!pokemonProj.isEvolving)
            {
                if (pokemonProj.UseEvoItem(GetType().Name))
                {
                    Item.consumable = true;
                    return true;
                }
            }
            Item.consumable = false;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PearlstoneBlock, 50)
				.AddIngredient(ItemID.Seashell, 5)
				.AddIngredient(ItemID.GoldCrown, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.PearlstoneBlock, 50)
                .AddIngredient(ItemID.Seashell, 5)
                .AddIngredient(ItemID.PlatinumCrown, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}