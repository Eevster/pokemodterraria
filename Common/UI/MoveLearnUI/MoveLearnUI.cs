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
		UIText confirmButton;
		public int selectedMove = -1;
		public string newMove;
		
		public CaughtPokemonItem pokemon;

		public void OpenPanel()
		{
			MoveLearnPanel = new DraggableUIPanel();
			MoveLearnPanel.SetPadding(0);

			SetRectangleAlign(MoveLearnPanel, left: 0.5f, top: 0.6f, width: 600, height: 300);

			var panelText = new UIText(Language.GetText("Mods.Pokemod.MoveLearnUI.MoveLearn").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.NPCs."+pokemon?.PokemonName+"CritterNPC.DisplayName")), 1f)
			{
				TextColor = Color.White,
				HAlign = 0.1f,
				VAlign = 0.16f,
				IsWrapped = true,
				Width = new(200, 0f),
				MinHeight = new(60, 0f),
				Height = new(60, 0f)
			};
			MoveLearnPanel.Append(panelText);

			Asset<Texture2D> pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + pokemon?.PokemonName); ;

			var PokemonImage = new UIImage(pokemonTexture)
			{
				ScaleToFit = true
			};
			SetRectangleAlign(PokemonImage, left: 0.12f, top: 0.65f, width: 120f, height: 120f);
			MoveLearnPanel.Append(PokemonImage);

			Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButton");
			Asset<Texture2D> buttonActiveTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButtonActive");

			for (int i = 0; i < movesButtons.Length; i++)
			{
				movesButtons[i] = new UIHoverImageButton(buttonTexture, buttonActiveTexture, Color.White, Language.GetTextValue("Move " + (i + 1)));
				movesButtons[i].canBeSelected = true;
				SetRectangleAlign(movesButtons[i], left: 0.8f, top: 0.08f + ((i == movesButtons.Length - 1) ? 0.7f : 0.16f * i), width: 292f, height: 42f);
				int moveIndex = i;
				movesButtons[i].OnLeftClick += (a, b) => SelectMove(moveIndex);

				var buttonText = new UIText(Language.GetText("Mods.Pokemod.Projectiles." + ((i == movesButtons.Length - 1) ? newMove : pokemon?.moves[i]) + ".DisplayName"), 1f)
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

			confirmButton = new UIText(Language.GetText("Mods.Pokemod.MoveLearnUI.Forget"), 1f)
			{
				TextColor = Color.Yellow,
				HAlign = 0.5f,
				VAlign = 1.05f,
				IsWrapped = true,
				Width = new(300, 0f),
				MinHeight = new(15, 0f),
				Height = new(15, 0f)
			};

			confirmButton.OnLeftClick += ConfirmButtonClicked;
			MoveLearnPanel.Append(confirmButton);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Camera_5");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
			SetRectangleAlign(closeButton, left: 0.975f, top: 0.05f, width: 32f, height: 32f);
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

			if (option != movesButtons.Length - 1) confirmButton.SetText(Language.GetTextValue("Mods.Pokemod.MoveLearnUI.Forget"));
			else confirmButton.SetText(Language.GetTextValue("Mods.Pokemod.MoveLearnUI.NotLearn"));

			movesButtons[option].selected = true;
		}

		private void ConfirmButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(SoundID.MenuOpen);

			List<string> newMoves = pokemon.moves.ToList();

			if (selectedMove < 4)
			{
				newMoves.RemoveAt(selectedMove);
				newMoves.Add(newMove);
			}
			pokemon.moves = newMoves.ToArray();

			ModContent.GetInstance<MoveLearnUISystem>().HideMyUI();
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<MoveLearnUISystem>().HideMyUI();
		}
	}
}