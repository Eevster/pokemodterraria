using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Pokemod.Content.Items;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.GloveAreaUI
{
    public class GloveUIElement : UIElement
    {
        private const string GloveTexturePath = "Pokemod/Assets/Textures/UI/TrainerGloveArea";
        private bool canDraw;

        public override void Update(GameTime gameTime)
        {
            canDraw = Main.player[Main.myPlayer].HeldItem.ModItem is TrainerGlove;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Draw nothing if cursor is disabled
            if (!canDraw)
            {
                return;
            }

            var tex = ModContent.Request<Texture2D>(GloveTexturePath).Value;
            spriteBatch.Draw(tex, Main.MouseWorld - Main.screenPosition, tex.Bounds, Color.White * 0.25f, 0f, tex.Size() / 2f, 1f * Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 1);
        }
    }
}