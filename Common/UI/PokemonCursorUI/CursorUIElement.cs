using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonCursorUI
{
    public class CursorUIElement : UIElement
    {
        private const string CursorTexturePath = "Pokemod/Assets/Textures/UI/PokemonCursor";
        private bool canDraw;

        public override void Update(GameTime gameTime)
        {
            if (Main.mapStyle == 2)
            {
                canDraw = false;
                return;
            }
            canDraw = Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().currentActivePokemon.Count > 0;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Draw nothing if cursor is disabled
            if (!canDraw)
            {
                return;
            }

            float ArrowDistance = ModContent.GetInstance<UIConfig>().ArrowDistance;
            float ArrowScale = ModContent.GetInstance<UIConfig>().ArrowScale;
            float PokemonImageDistance = ModContent.GetInstance<UIConfig>().PokemonImageDistance;

            // Get the player position
            var playerPos = Main.player[Main.myPlayer].Center;

            //Get UI scale and prepare scaling factor
            var posScaleFactor = 1f / Main.UIScale;

            // Draw an arrow for each pokemon
            foreach (int index in Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().currentActivePokemon)
            {
                if (Main.projectile[index].active)
                {
                    PokemonPetProjectile PokemonProj;

                    if (Main.projectile[index].ModProjectile is PokemonPetProjectile) PokemonProj = (PokemonPetProjectile)Main.projectile[index]?.ModProjectile;
                    else PokemonProj = null;

                    if(PokemonProj != null)
                    {
                        var p = PokemonProj.Projectile.Center - Main.screenPosition;
                        if (!(p.X < 0 || p.Y < 0 || p.X > Main.screenWidth * Main.UIScale || p.Y > Main.screenHeight * Main.UIScale))
                        {
                            continue;
                        }
                    }

                    var pokemonVector = PokemonProj.Projectile.Center - playerPos;
                    
                    // reverse arrow if gravitation potion effect is active
                    pokemonVector.Y *= Main.LocalPlayer.gravDir;
                    
                    // Defines variables used to for drawing
                    var modifier = Math.Clamp(1.15f - 1 / (2f * Main.screenWidth) * pokemonVector.Length(), 0.02f, 1f);
                    var alpha = modifier * 0.9f;
                    var scale = modifier * 1.2f ;
                    pokemonVector.Normalize();
                    var arrowPos = playerPos + pokemonVector * ArrowDistance - Main.screenPosition;
                    arrowPos *= posScaleFactor;

                    var rotation = (float) Math.Atan2(pokemonVector.Y, pokemonVector.X);
                    
                    // Draw the arrow
                    var tex = ModContent.Request<Texture2D>(CursorTexturePath).Value;
                    spriteBatch.Draw(tex, arrowPos, null, PokemonProj.GetHPBarColor() * alpha, rotation, tex.Size() / 2f, 1.2f * ArrowScale, SpriteEffects.None, 1);
                    var texBorder = ModContent.Request<Texture2D>(CursorTexturePath + "_Border").Value;
                    spriteBatch.Draw(texBorder, arrowPos, null, Color.White * alpha, rotation, tex.Size() / 2f, 1.2f * ArrowScale, SpriteEffects.None, 1);

                    // Draw the pokemon head
                    var headTex = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/"+PokemonProj.Name.Replace("PetProjectile","")).Value;
                    var headPos = playerPos + pokemonVector * (ArrowDistance - (PokemonImageDistance * Main.UIScale) * ArrowScale) - Main.screenPosition;
                    headPos *= posScaleFactor;
                    spriteBatch.Draw(
                        headTex,
                        headPos,
                        null,
                        Color.White * alpha,
                        0f,
                        headTex.Size() * 0.5f,
                        scale * ArrowScale,
                        SpriteEffects.None,
                        0);
                }
            }
        }
    }
}