using Pokemod.Content.Projectiles.Tools;
using Pokemod.Common.UI.PokedexUI;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Tools
{
	public class PokedexV1 : ModItem
	{
		private static Asset<Texture2D> openTexture;

		public override void Load() {
			openTexture = ModContent.Request<Texture2D>(Texture + "_Open");
		}
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.MenuTick;
			Item.value = Item.sellPrice(gold: 1);
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return true;
			}

			if (PokedexUIState.hidden)
			{
				ModContent.GetInstance<PokedexUISystem>().ShowMyUI();
			}

			return true;
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if(!PokedexUIState.hidden)
			{
				spriteBatch.Draw(openTexture.Value,
					position: position-scale*new Vector2(openTexture.Value.Width/2, openTexture.Value.Height/2),
					sourceRectangle: openTexture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: scale,
					SpriteEffects.None,
					layerDepth: 0f);
			}
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<RedApricorn>(), 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, 20)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}