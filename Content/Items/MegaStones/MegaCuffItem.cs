using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.Buffs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class MegaCuffItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28; // The item texture's width
			Item.height = 28; // The item texture's height
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noUseGraphic = true;
			Item.maxStack = 1; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}
		
		public override bool? UseItem(Player player)
		{
            if (player.whoAmI != Main.myPlayer) {
				return true;
			}

			if (player.GetModPlayer<PokemonPlayer>().CanMegaEvolve != 0)
			{
				SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				CombatText.NewText(player.Hitbox, new Color(255, 255, 255), "Not Charged!");
				return true;
			}

			if (!player.GetModPlayer<PokemonPlayer>().HasPokemonByName(player.GetModPlayer<PokemonPlayer>().MegaStone.Replace("MegaStoneItem","").Replace("MegaStoneItemX","").Replace("MegaStoneItemY","")))
			{
				SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				CombatText.NewText(player.Hitbox, new Color(255, 255, 255), "Incorrect Pokemon");
				return true;
			}

			if (player.GetModPlayer<PokemonPlayer>().HasMegaStone > 0)
			{
				SoundEngine.PlaySound(SoundID.Item6, player.position);
				player.AddBuff(ModContent.BuffType<MegaEvolution>(), 60 * 60);
				player.GetModPlayer<PokemonPlayer>().CanMegaEvolve = 2;
			}

            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if (Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().CanMegaEvolve == 0)
			{
				Asset<Texture2D> megaTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/MegaItemSymbol");

				spriteBatch.Draw(megaTexture.Value,
					position: position,
					sourceRectangle: megaTexture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: megaTexture.Size() / 2,
					scale: scale,
					SpriteEffects.None,
					layerDepth: 0f);
			}
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<KeyStoneItem>(1)
				.AddIngredient(ItemID.HallowedBar, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
