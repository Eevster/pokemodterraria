using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Systems;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
		public int itemUsedType = -1;
		public int itemUsedAmount = 1;

		public static bool hidden = true;
		
		public CaughtPokemonItem pokemon;

		public int timer = 0;

		public void OpenPanel()
		{
			RemoveAllChildren();
			selectedMove = -1;

			MoveLearnPanel = new DraggableUIPanel();
			MoveLearnPanel.SetPadding(0);
            UIHelpers.SetRectangleAlign(MoveLearnPanel, left: 0.8f, top: 0.2f, width: 600, height: 300);

			var leftSidePanel = new UIPanel();
			leftSidePanel.BorderColor = new(0, 0, 0, 0);
            leftSidePanel.BackgroundColor = new(0, 0, 0, 0);
			leftSidePanel.IgnoresMouseInteraction = true;
            leftSidePanel.SetPadding(0);
            UIHelpers.SetRectangle(leftSidePanel, left: 13, top: 13, width: 224, height: MoveLearnPanel.Height.Pixels - 26);

			//Pokemon Icon
            Asset<Texture2D> iconFrameTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/PokemonIconFrame");
            Asset <Texture2D> pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + pokemon?.PokemonName);
			var PokemonFrame = new UIImage(iconFrameTexture) { Color = new(255, 255, 255, 80) };
            UIHelpers.SetRectangleAlign(PokemonFrame, left: 0.5f, top: 0f, width: 104f, height: 104f);
            var PokemonImage = new UIImage(pokemonTexture) { ScaleToFit = true };
            UIHelpers.SetRectangleAlign(PokemonImage, left: 0.5f, top: 0.5f, width: 96f, height: 96f);

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
			UIHelpers.SetRectangle(confirmButton, left: 0.5f, top: 1f, width: 168, height: 46);
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

                movesButtons[i] = new UIHoverImageButton(moveButtonTexture, moveButtonActiveTexture, Color.White, PokemonData.SetMoveTooltip(pokemon, moveName));
				movesButtons[i].tooltipSolid = true;
				movesButtons[i].canBeSelected = true;
				UIHelpers.SetRectangleAlign(movesButtons[i], left: 1f, top: isNewMove ? 1f : 0f, width: 292f, height: 42f);
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

		public void SetMoveData(CaughtPokemonItem pokemon, string newMove, int itemUsedType = -1, int itemUsedAmount = 1)
		{
			this.pokemon = pokemon;
			this.newMove = newMove;
			this.itemUsedType = itemUsedType;
			this.itemUsedAmount = itemUsedAmount;
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
			}
			pokemon.moves = newMoves.ToArray();

			if (selectedMove == 4) RefundItemUsed();
			else pokemon.MoveLearnEffects(newMove);

			TownNPCRespawnSystem.unlockedMoveTutor = true;

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
				RefundItemUsed();
                TownNPCRespawnSystem.unlockedMoveTutor = true;
                ModContent.GetInstance<MoveLearnUISystem>().HideMyUI();
            }
		}

		public void RefundItemUsed()
		{
			string notLearnMessage = newMove + " not learned. ";
			if (itemUsedType != -1) 
			{
				Player player = Main.LocalPlayer;
				if (player != null)
				{
					var entitySource = new EntitySource_Misc("RefundUsedItem");
					int item = player.QuickSpawnItem(entitySource, itemUsedType, itemUsedAmount);
					notLearnMessage += Main.item[item].HoverName + " refunded.";
				}
			}
			Main.NewText(notLearnMessage);
		}
	}
}