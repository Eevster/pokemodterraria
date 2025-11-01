using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.Items.Tools;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Pokemod.Common.UI.StarterPanelUI
{
	public class StarterPanelUIState : UIState
	{
		public DraggableUIPanel StarterBoxPanel;

		UIHoverPokeballButton firstStarterButton;
		UIHoverPokeballButton secondStarterButton;
		UIHoverPokeballButton thirdStarterButton;

		public string[] starters;

		public override void OnInitialize() {
			starters = PokemonNPCData.GetStarters();

			Asset<Texture2D> StarterBoxTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/StarterPanel");
			StarterBoxPanel = new DraggableUIImagePanel(StarterBoxTexture, Color.White);
			StarterBoxPanel.SetPadding(0);

			SetRectangleAlign(StarterBoxPanel, left: 0.5f, top: 0.6f, width: 900, height: 600);

			firstStarterButton = new UIHoverPokeballButton(starters[0], Color.White, Language.GetTextValue("First Starter"));
			firstStarterButton.drawPanel = false;
			firstStarterButton.hoverUp = 40f;
			SetRectangleAlign(firstStarterButton, left: 0.1f, top: 0.6f, width: 244f, height: 244f);
			firstStarterButton.OnLeftClick +=  (a, b) => GetStarter(0);
			StarterBoxPanel.Append(firstStarterButton);

			secondStarterButton = new UIHoverPokeballButton(starters[1], Color.White, Language.GetTextValue("Second Starter"));
			secondStarterButton.drawPanel = false;
			secondStarterButton.hoverUp = 40f;
			SetRectangleAlign(secondStarterButton, left: 0.5f, top: 0.47f, width: 244f, height: 244f);
			secondStarterButton.OnLeftClick +=  (a, b) => GetStarter(1);
			StarterBoxPanel.Append(secondStarterButton);

			thirdStarterButton = new UIHoverPokeballButton(starters[2], Color.White, Language.GetTextValue("Third Starter"));
			thirdStarterButton.drawPanel = false;
			thirdStarterButton.hoverUp = 40f;
			SetRectangleAlign(thirdStarterButton, left: 0.9f, top: 0.6f, width: 244f, height: 244f);
			thirdStarterButton.OnLeftClick +=  (a, b) => GetStarter(2);
			StarterBoxPanel.Append(thirdStarterButton);

			var rerollButton = new UIText(Language.GetText("Reroll"), 1.5f) {
				TextColor = Color.Yellow,
				HAlign = 0.5f,
				VAlign = 0.83f,
				IsWrapped = true,
				Width = new(100, 0f),
				MinHeight = new(15, 0f),
				Height = new(15, 0f)
			};

			rerollButton.OnLeftClick += RerollButtonClicked;
			StarterBoxPanel.Append(rerollButton);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Camera_5");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
			SetRectangleAlign(closeButton, left: 0.95f, top: 0.1f, width: 32f, height: 32f);
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
			StarterBoxPanel.Append(closeButton);

			Append(StarterBoxPanel);
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

		private void GetStarter(int option){
			Player localPlayer = Main.player[Main.myPlayer];
			localPlayer.GetModPlayer<PokemonPlayer>().GenerateCaughtPokemon(starters[option]);
			localPlayer.GetModPlayer<PokemonPlayer>().HasStarter = true;
			localPlayer.QuickSpawnItem(localPlayer.GetSource_FromThis(), ModContent.ItemType<PokedexV1>());
			ModContent.GetInstance<StarterPanelUISystem>().HideMyUI();
		}

		private void RerollButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuOpen);
			starters = PokemonNPCData.GetStarters();
			firstStarterButton.SetPokemon(starters[0]);
			secondStarterButton.SetPokemon(starters[1]);
			thirdStarterButton.SetPokemon(starters[2]);
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<StarterPanelUISystem>().HideMyUI();
		}
	}
}