
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria.Localization;

namespace Pokemod.Content.Items.Consumables
{
	public abstract class ExpCandy : PokemonConsumableItem
	{
		public virtual int expAmount => 100;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(expAmount);

		public override void SetDefaults() {
			Item.width = 24; // The item texture's width
			Item.height = 24; // The item texture's height

			Item.useTime = 1;
			Item.useAnimation = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item1;

			Item.rare = ItemRarityID.Expert;

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.

			Item.consumable = true;
		}

        public override bool OnItemUse(Projectile proj){
			PokemonPetProjectile pokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			if(pokemonProj.pokemonLvl < 100){
				pokemonProj.SetGainedExp(expAmount);
				Item.consumable = true;
				return true;
			}
			Item.consumable = false;
			return false;
		}

		public override bool OnItemInvUse(CaughtPokemonItem item, Player player){
            if(item.level < 100){
                item.exp += expAmount;
				ReduceStack(player, Item.type);
                return true;
            }
            return false;
		}
	}

	public class ExpCandyXS : ExpCandy {}
	public class ExpCandyS : ExpCandy
	{
        public override int expAmount => 800;

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExpCandyXS>(8)
                .AddTile(TileID.Bottles)
                .Register();
        }
	}
	public class ExpCandyM : ExpCandy
	{
        public override int expAmount => 3000;

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExpCandyS>(4)
                .AddTile(TileID.Bottles)
                .Register();
        }
	}
	public class ExpCandyL : ExpCandy
	{
        public override int expAmount => 10000;
        public override void SetDefaults()
        {
            base.SetDefaults();
			Item.width = 28;
			Item.height = 28;
        }

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExpCandyM>(4)
                .AddTile(TileID.Bottles)
                .Register();
        }
	}
	public class ExpCandyXL : ExpCandy
	{
        public override int expAmount => 30000;
        public override void SetDefaults()
        {
            base.SetDefaults();
			Item.width = 32;
			Item.height = 32;
        }

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExpCandyL>(3)
                .AddTile(TileID.Bottles)
                .Register();
        }
	}
}
