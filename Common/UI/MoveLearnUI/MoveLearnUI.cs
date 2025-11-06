using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.MoveLearnUI
{
	public class MoveLearnUIState : UIState
	{
		public DraggableUIPanel MoveLearnPanel;

		UIHoverImageButton[] movesButtons = new UIHoverImageButton[5];
		UIHoverImageButton confirmButton;
		UIText confirmText;
		public int selectedMove = -1;
		public string newMove;

		public static bool hidden = true;
		
		public CaughtPokemonItem pokemon;

		public int timer = 0;

		public void OpenPanel()
		{
			RemoveAllChildren();
			selectedMove = -1;

			MoveLearnPanel = new DraggableUIPanel();
			MoveLearnPanel.SetPadding(0);
			SetRectangleAlign(MoveLearnPanel, left: 0.8f, top: 0.2f, width: 600, height: 300);

			var leftSidePanel = new UIPanel();
			leftSidePanel.BorderColor = new(0, 0, 0, 0);
            leftSidePanel.BackgroundColor = new(0, 0, 0, 0);
			leftSidePanel.IgnoresMouseInteraction = true;
            leftSidePanel.SetPadding(0);
			SetRectangle(leftSidePanel, left: 13, top: 13, width: 224, height: MoveLearnPanel.Height.Pixels - 26);

			//Pokemon Icon
            Asset<Texture2D> iconFrameTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/PokemonIconFrame");
            Asset <Texture2D> pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + pokemon?.PokemonName);
			var PokemonFrame = new UIImage(iconFrameTexture) { Color = new(255, 255, 255, 80) };
            SetRectangleAlign(PokemonFrame, left: 0.5f, top: 0f, width: 104f, height: 104f);
            var PokemonImage = new UIImage(pokemonTexture) { ScaleToFit = true };
			SetRectangleAlign(PokemonImage, left: 0.5f, top: 0.5f, width: 96f, height: 96f);

			PokemonFrame.Append(PokemonImage);
            leftSidePanel.Append(PokemonFrame);

			//Confirm Button
			confirmButton = new UIHoverImageButton(ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/GoldButton"), Language.GetTextValue("Mods.Pokemod.MoveLearnUI.NoSelectionTooltip"))
			{
				Width = new(168, 0),
				Height = new(46, 0),

				VAlign = 1f,
				MarginBottom = 14,
				MarginLeft = 14 + (leftSidePanel.Width.Pixels / 2) - (168 / 2),
				canBeSelected = true,
			};
			SetRectangle(confirmButton, left: 0.5f, top: 1f, width: 168, height: 46);
            confirmButton.OnLeftClick += ConfirmButtonClicked;

            confirmText = new UIText(Language.GetTextValue("Mods.Pokemod.MoveLearnUI.NoSelection"), 1f)
            {
                TextColor = new(120,120,120,80),
                HAlign = 0.5f,
                VAlign = 0.5f,
                OverflowHidden = true,
                Width = new(300, 0f),
                MinHeight = new(15, 0f),
                Height = new(15, 0f)
            };
            confirmButton.Append(confirmText);
            MoveLearnPanel.Append(confirmButton);

            //Text explanation
            float spaceForText = leftSidePanel.Height.Pixels - PokemonFrame.Height.Pixels - confirmButton.Height.Pixels - 20f;
			var panelText = new UIText(Language.GetText("Mods.Pokemod.MoveLearnUI.MoveLearn").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.NPCs." + pokemon?.PokemonName + "CritterNPC.DisplayName")), 1f)
			{
				HAlign = 0.5f,
				MarginTop = PokemonFrame.Height.Pixels + 10,
				Width = new(224, 0f),
                MinHeight = new(spaceForText, 0f),
                Height = new(spaceForText, 0f),

				TextColor = Color.White,
				IsWrapped = true,
                WrappedTextBottomPadding = 0,
                TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
            leftSidePanel.Append(panelText);
            MoveLearnPanel.Append(leftSidePanel);

            //Move Buttons
            Asset<Texture2D> moveButtonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButton");
			Asset<Texture2D> moveButtonActiveTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButtonActive");

			for (int i = 0; i < movesButtons.Length; i++)
			{
				bool isNewMove = (i == movesButtons.Length - 1);
				string moveName = (isNewMove ? newMove : pokemon?.moves[i]);

                movesButtons[i] = new UIHoverImageButton(moveButtonTexture, moveButtonActiveTexture, Color.White, SetMoveTooltip(pokemon, moveName));
				movesButtons[i].tooltipSolid = true;
				movesButtons[i].canBeSelected = true;
				SetRectangleAlign(movesButtons[i], left: 1f, top: isNewMove ? 1f : 0f, width: 292f, height: 42f);
				if (isNewMove) movesButtons[i].MarginBottom = 16f;
				else movesButtons[i].MarginTop = 14 + (isNewMove ? 0f : (8 + movesButtons[i].Height.Pixels) * i);
				movesButtons[i].MarginRight = 56; //Margin of 14 + Close button dimension 0f 32 + 10 pixel gap.
                int moveIndex = i;
				movesButtons[i].OnLeftClick += (a, b) => SelectMove(moveIndex);

				var buttonText = new UIText(Language.GetText("Mods.Pokemod.Projectiles." + moveName + ".DisplayName"), 1f)
				{
					TextColor = Color.White,
					HAlign = 0.5f,
					VAlign = 0.5f,
					Width = new(292f, 0f),
					Height = new(21f, 0f),
				};
				movesButtons[i].Append(buttonText);
				MoveLearnPanel.Append(movesButtons[i]);
			}

			//Close Button
			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Camera_5");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")) { }; // Localized text for "Close"
			closeButton.HAlign = 1f;
			closeButton.MarginRight = 14f;
			closeButton.MarginTop = 14f;
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
			MoveLearnPanel.Append(closeButton);

			Append(MoveLearnPanel);
		}

        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height) {
			uiElement.Left.Set(left, 0f);
			uiElement.Top.Set(top, 0f);
			uiElement.Width.Set(width, 0f);
			uiElement.Height.Set(height, 0f);
		}

		private void SetRectangleAlign(UIElement uiElement, float left, float top, float width, float height) {
			uiElement.HAlign = left;
			uiElement.VAlign = top;
			uiElement.Width.Set(width, 0f);
			uiElement.Height.Set(height, 0f);
		}

		private string SetMoveTooltip(CaughtPokemonItem pokemon, string moveName)
		{
            int moveType = PokemonData.pokemonAttacks[moveName].attackType;
            int moveSpeed = (int)(6000f / Math.Clamp(PokemonData.pokemonAttacks[moveName].cooldown + PokemonData.pokemonAttacks[moveName].attackDuration, 1, 1200));

            //Type, Special, Power, Speed, Range, (Effect? Maybe relevant descriptions could be added to Pokemon Data)
            string moveToolTip = "[c/" + PokemonNPCData.GetTypeColor(moveType) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonTypes." + (TypeIndex)moveType) + "]\n"
                + (PokemonData.pokemonAttacks[moveName].isSpecial ? "Special" : "Physical") + "\n"
                + Language.GetText("Mods.Pokemod.MoveLearnUI.MovePower").WithFormatArgs(PokemonData.pokemonAttacks[moveName].attackPower).ToString() + "\n"
                + Language.GetText("Mods.Pokemod.MoveLearnUI.MoveSpeed").WithFormatArgs(moveSpeed).ToString() + "\n"
                + Language.GetText("Mods.Pokemod.MoveLearnUI.MoveRange").WithFormatArgs((int)(PokemonData.pokemonAttacks[moveName].distanceToAttack / 16)).ToString() + "\n";
			return moveToolTip;
		}

		public void SetMoveData(CaughtPokemonItem pokemon, string newMove)
		{
			this.pokemon = pokemon;
			this.newMove = newMove;
			OpenPanel();
		}

		private void SelectMove(int option)
		{
			selectedMove = option;

			foreach (UIHoverImageButton button in movesButtons)
			{
				button.selected = false;
			}

            movesButtons[option].selected = true;
			bool learn = true;
            if (option == movesButtons.Length - 1) learn = false;
            UpdateConfirmText(learn, option);
            confirmButton.SetHoverImage(ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/GoldButtonHover"));
        }

		public void UpdateConfirmText(bool learn, int selection)
		{
			confirmText.TextColor = Color.Yellow;
			if (learn)
			{
				confirmText.SetText(Language.GetTextValue("Mods.Pokemod.MoveLearnUI.Forget"));
                confirmButton.tooltipText = Language.GetText("Mods.Pokemod.MoveLearnUI.ForgetTooltip").WithFormatArgs(newMove, pokemon?.moves[selection]).ToString();
			}
			else
			{
				confirmText.SetText(Language.GetTextValue("Mods.Pokemod.MoveLearnUI.NotLearn"));
                confirmButton.tooltipText = Language.GetText("Mods.Pokemod.MoveLearnUI.NotLearnTooltip").WithFormatArgs(newMove).ToString();
            }
		}

        private void ConfirmButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			if (selectedMove == -1) return;

			SoundEngine.PlaySound(SoundID.MenuOpen);

			List<string> newMoves = pokemon.moves.ToList();

			if (selectedMove < 4)
			{
				newMoves.RemoveAt(selectedMove);
				newMoves.Add(newMove);
                pokemon.MoveLearnEffects(newMove);
            }
			pokemon.moves = newMoves.ToArray();

			ModContent.GetInstance<MoveLearnUISystem>().HideMyUI();
		}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (timer < 45) timer++;
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			if (timer >= 45)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				ModContent.GetInstance<MoveLearnUISystem>().HideMyUI();
			}
		}
	}
}