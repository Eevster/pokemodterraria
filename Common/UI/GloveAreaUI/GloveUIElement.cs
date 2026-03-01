using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Pokemod.Content.Items;
using Pokemod.Content.Items.TrainerGear;
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

            float trainerGloveRange = Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().trainerGloveRange;
            bool enemyInRange = false;

            for (int i = 0; i < Main.maxNPCs; i++){
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy() && npc.damage != 0){
                    if(Vector2.Distance(npc.Center, Main.MouseWorld) <= 16f*trainerGloveRange)
                    {
                        enemyInRange = true;
                        break;
                    }
                }
            }

            var tex = ModContent.Request<Texture2D>(GloveTexturePath).Value;
            spriteBatch.Draw(tex, Main.MouseWorld - Main.screenPosition, tex.Bounds, Color.White * (enemyInRange?0.4f:0.1f), 0f, tex.Size() / 2f, (trainerGloveRange/10f) * (Main.GameZoomTarget / Main.UIScale), SpriteEffects.None, 1);
        }
    }
}