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

        UIImage pokemonBar;
        UIImageFlip pokemonEnemyBar;

        UIElement barPanel;
        UIElement barEnemyPanel;
        
        public override void OnInitialize()
        {
            Asset<Texture2D> pokemonBarImage = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/BattlePokemonBar");
            pokeballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/BattlePokeball");

            UIImage pokemonBar = new UIImage(pokemonBarImage) {};
            //UIHelpers.SetRectangle(pokemonBar, left: playerBarFrame.Left-74, top: playerBarFrame.Top-34, width: pokemonBarImage.Width(), height: pokemonBarImage.Height());
            UIHelpers.SetRectangleAlign(pokemonBar, left: 0.5f, top: 0f, width: barFrameWidth, height: barFrameHeight);
            pokemonBar.Left.Set(-barFrameWidth/2 - barSeparation/2, 0);
            pokemonBar.Top.Set(100, 0);
            barPanel = new UIElement();
            UIHelpers.SetRectangle(barPanel, left: 74, top: 34, width: barWidth, height: barHeight);
            pokemonBar.Append(barPanel);
            Append(pokemonBar);

            pokemonEnemyBar = new UIImageFlip(pokemonBarImage){flipX = true};
            //UIHelpers.SetRectangle(pokemonEnemyBar, left: enemyBarFrame.Left-14, top: enemyBarFrame.Top-34, width: pokemonBarImage.Width(), height: pokemonBarImage.Height());
            UIHelpers.SetRectangleAlign(pokemonEnemyBar, left: 0.5f, top: 0f, width: barFrameWidth, height: barFrameHeight);
            pokemonEnemyBar.Left.Set(barFrameWidth/2 + barSeparation/2, 0);
            pokemonEnemyBar.Top.Set(100, 0);
            barEnemyPanel = new UIElement();
            UIHelpers.SetRectangle(barEnemyPanel, left: 14, top: 34, width: barWidth, height: barHeight);
            pokemonEnemyBar.Append(barEnemyPanel);
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
            Rectangle playerBarFrame = barPanel.GetInnerDimensions().ToRectangle();
            Rectangle enemyBarFrame = barEnemyPanel.GetInnerDimensions().ToRectangle();
            
            if(playerPokemon != null)
            {
                if(playerPokemon.Projectile.active) DrawHPBar(spriteBatch, playerPokemon, playerBarFrame);
                else playerPokemon = null;
            }

            if(enemyPokemon != null)
            {
                if(enemyPokemon.Projectile.active) DrawHPBar(spriteBatch, enemyPokemon, enemyBarFrame, true);
                else enemyPokemon = null;
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
    }
}