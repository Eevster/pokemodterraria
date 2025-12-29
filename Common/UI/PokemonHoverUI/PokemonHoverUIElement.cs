using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonHoverUI
{
    public class PokemonHoverUIElement : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var player = Main.player[Main.myPlayer];

            //Get UI scale and prepare scaling factor
            var posScaleFactor = 1f / Main.UIScale;

            foreach(Projectile proj in Main.projectile){
                if(proj.owner == player.whoAmI){
                    if(proj.ModProjectile != null){
                        if(proj.active){
                            if(proj.ModProjectile is PokemonPetProjectile pokemon){
                                Vector2 mousePosition = Main.MouseWorld;
                                if (Vector2.Distance(proj.Center, Main.UIScale*(mousePosition-Main.screenPosition)+Main.screenPosition) <= 64f){
                                    string PokemonInfo = pokemon.pokemonName + " Lvl " + pokemon.pokemonLvl;
                                    DynamicSpriteFont font = (DynamicSpriteFont)FontAssets.MouseText;
                                    Vector2 vector2 = font.MeasureString(PokemonInfo);
                                    Vector2 vector3 = new Vector2(vector2.X * 0.5f, vector2.Y * 0.5f);
                                    var infoPos = posScaleFactor*(proj.Top - Main.screenPosition) - vector3 + new Vector2(0, -40*posScaleFactor);
                    
                                    DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, PokemonInfo, infoPos, Color.White);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}