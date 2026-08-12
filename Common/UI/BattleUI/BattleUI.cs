using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.BattleUI
{
    public class BattleUI : UIState
    {
        public PokemonPetProjectile playerPokemon;
        public PokemonPetProjectile enemyPokemon; 
        public UIText currentMove;

        Asset<Texture2D> pokeballTexture;

        int barFrameWidth = 388;
        int barFrameHeight = 60;

        int barHeight = 10;
        int barWidth = 300;

        int barSeparation = 200;

        // Player UI Vars
        UIImage pokemonBar;
        UIElement infoPanel;
        UIElement barPanel;
        UIElement iconPanel;

        int teamCount = 6;
        int defeatedCount = 2;

        // Enemy UI Vars
        UIImageFlip pokemonEnemyBar;
        UIElement infoEnemyPanel;
        UIElement barEnemyPanel;
        UIElement iconEnemyPanel;

        int teamEnemyCount = 6;
        int defeatedEnemyCount = 3;
        
        public override void OnInitialize()
        {
            Asset<Texture2D> pokemonBarImage = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/BattlePokemonBar");
            pokeballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/BattlePokeball");

            // Player UI Elements
            pokemonBar = new UIImage(pokemonBarImage) {};
            //UIHelpers.SetRectangle(pokemonBar, left: playerBarFrame.Left-74, top: playerBarFrame.Top-34, width: pokemonBarImage.Width(), height: pokemonBarImage.Height());
            UIHelpers.SetRectangleAlign(pokemonBar, left: 0.5f, top: 0f, width: barFrameWidth, height: barFrameHeight);
            pokemonBar.Left.Set(-barFrameWidth/2 - barSeparation/2, 0);
            pokemonBar.Top.Set(100, 0);

            barPanel = new UIElement();
            UIHelpers.SetRectangle(barPanel, left: 74, top: 34, width: barWidth, height: barHeight);
            pokemonBar.Append(barPanel);

            infoPanel = new UIElement();
            UIHelpers.SetRectangle(infoPanel, left: 60, top: 0, width: 328, height: 28);
            pokemonBar.Append(infoPanel);

            iconPanel = new UIElement();
            UIHelpers.SetRectangle(iconPanel, left: 0, top: 0, width: 60, height: 60);
            pokemonBar.Append(iconPanel);

            Append(pokemonBar);

            // Enemy UI Elements
            pokemonEnemyBar = new UIImageFlip(pokemonBarImage){flipX = true};
            //UIHelpers.SetRectangle(pokemonEnemyBar, left: enemyBarFrame.Left-14, top: enemyBarFrame.Top-34, width: pokemonBarImage.Width(), height: pokemonBarImage.Height());
            UIHelpers.SetRectangleAlign(pokemonEnemyBar, left: 0.5f, top: 0f, width: barFrameWidth, height: barFrameHeight);
            pokemonEnemyBar.Left.Set(barFrameWidth/2 + barSeparation/2, 0);
            pokemonEnemyBar.Top.Set(100, 0);

            barEnemyPanel = new UIElement();
            UIHelpers.SetRectangle(barEnemyPanel, left: 14, top: 34, width: barWidth, height: barHeight);
            pokemonEnemyBar.Append(barEnemyPanel);

            infoEnemyPanel = new UIElement();
            UIHelpers.SetRectangle(infoEnemyPanel, left: 0, top: 0, width: 328, height: 28);
            pokemonEnemyBar.Append(infoEnemyPanel);

            iconEnemyPanel = new UIElement();
            UIHelpers.SetRectangle(iconEnemyPanel, left: 328, top: 0, width: 60, height: 60);
            pokemonEnemyBar.Append(iconEnemyPanel);

            Append(pokemonEnemyBar);

			var helpText = new UIText(Language.GetTextValue("Mods.Pokemod.PokemonBattle.ToAttack")+" - "+Language.GetTextValue("Mods.Pokemod.PokemonBattle.ToSwitchMove"), 1f)
			{
				TextColor = Color.White,
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};

            UIHelpers.SetRectangleAlign(helpText, left: 0.5f, top: 0.92f, width: 400, height: 80);

			Append(helpText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {            
            if(playerPokemon != null)
            {
                if(playerPokemon.Projectile.active){
                    DrawHPBar(spriteBatch, playerPokemon, barPanel.GetInnerDimensions().ToRectangle());
                    DrawPokemonIcon(spriteBatch, playerPokemon, iconPanel.GetInnerDimensions().ToRectangle());
                }
                else playerPokemon = null;
            }

            if(enemyPokemon != null)
            {
                if(enemyPokemon.Projectile.active){
                    DrawHPBar(spriteBatch, enemyPokemon, barEnemyPanel.GetInnerDimensions().ToRectangle(), true);
                    DrawPokemonIcon(spriteBatch, enemyPokemon, iconEnemyPanel.GetInnerDimensions().ToRectangle());
                }
                else enemyPokemon = null;
            }

            for(int i = 0; i < teamCount; i++)
            {
                spriteBatch.Draw(pokeballTexture.Value, iconPanel.GetInnerDimensions().ToRectangle().BottomRight() + new Vector2(4+pokeballTexture.Value.Width*i+2*i,2), pokeballTexture.Value.Bounds, i<defeatedCount?Color.Black:Color.White);
            }

            for(int i = 0; i < teamEnemyCount; i++)
            {
                spriteBatch.Draw(pokeballTexture.Value, iconEnemyPanel.GetInnerDimensions().ToRectangle().BottomLeft() + new Vector2(-(4+pokeballTexture.Value.Width*(i+1)+2*i),2), pokeballTexture.Value.Bounds, i<defeatedEnemyCount?Color.Black:Color.White);
            }
        }

        private void DrawHPBar(SpriteBatch spriteBatch, PokemonPetProjectile pokemon, Rectangle frame, bool inverted = false)
        {
            // Calculate quotient
            float quotient = (float)pokemon.currentHp / pokemon.finalStats[0];
            quotient = Utils.Clamp(quotient, 0f, 1f);

            int left = frame.Left;
            int right = frame.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                if(!inverted) spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, frame.Y, 1, frame.Height), pokemon.GetHPBarColor());
                else spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(right - 1 - i, frame.Y, 1, frame.Height), pokemon.GetHPBarColor());
            }
        }

        private void DrawPokemonIcon(SpriteBatch spriteBatch, PokemonPetProjectile pokemon, Rectangle frame)
        {
            if (ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Icons/" + pokemon.pokemonName + (pokemon.Name.Contains("Shiny")?"Shiny":""), out Asset<Texture2D> pokeTexture))
            {
                spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                spriteBatch.Draw(pokeTexture.Value, frame.Center(), pokeTexture.Value.Bounds, Color.White, 0f, pokeTexture.Size()*0.5f, 2f, SpriteEffects.None, 0);
            }
        }

        public void UpdateMove(string move)
        {
            string moveText = ">>>"+"[c/" + PokemonNPCData.GetTypeColor(PokemonData.pokemonAttacks[move].attackType) + ":" + Language.GetText("Mods.Pokemod.Projectiles." + move + ".DisplayName") + "]"+"<<<";

            if (HasChild(currentMove))
            {
                RemoveChild(currentMove);
            }

            currentMove = new UIText(moveText, 1f)
			{
				TextColor = Color.White,
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};

            UIHelpers.SetRectangleAlign(currentMove, left: 0.5f, top: 0.95f, width: 400, height: 80);
            Append(currentMove);
        }

        public void SetPlayerPokemon(PokemonPetProjectile pokemon)
        {
            playerPokemon = pokemon;
            UpdatePokemonInfo(pokemon);
        }

        public void SetEnemyPokemon(PokemonPetProjectile pokemon)
        {
            enemyPokemon = pokemon;
            UpdatePokemonInfo(pokemon, true);
        }

        public void UpdatePokemonInfo(PokemonPetProjectile pokemon, bool isEnemy = false)
        {
            if(!isEnemy)infoPanel.RemoveAllChildren();
            else infoEnemyPanel.RemoveAllChildren();

            if(pokemon != null && pokemon.Projectile.active)
            {
                var pokemonNameText = new UIText(Language.GetTextValue("Mods.Pokemod.NPCs." + pokemon.pokemonName + "CritterNPC.DisplayName") + ((!pokemon.pokemonName.Contains("Nidoran") && pokemon.gender != 0)?(pokemon.gender==1?" ♂":" ♀"):"") + " Lvl " + pokemon.pokemonLvl, 1f)
                {
                    TextColor = Color.White,
                    TextOriginX = 0f,
                    TextOriginY = 0.5f,
                };
                UIHelpers.SetRectangle(pokemonNameText, left: 14, top: 8, width: 154, height: 16);

                if(!isEnemy)infoPanel.Append(pokemonNameText);
                else infoEnemyPanel.Append(pokemonNameText);
            }
        }

        public void SetTeamInitialInfo(int total, bool isEnemy = false)
        {
            if (!isEnemy){
                teamCount = total;
                defeatedCount = 0;
            }
            else
            {
                teamEnemyCount = total;
                defeatedEnemyCount = 0;
            }
        }

        public void SetDefeatedPokemon(bool isEnemy = false)
        {
            if(!isEnemy) defeatedCount++;
            else defeatedEnemyCount++;
        }
    }
}