using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokedexUI
{
	public class PokedexUIState : UIState
	{
		public DraggableUIPanel PokedexPanel;

		public UIPanel ListPanel;
		public UIPanel InfoPanel;

		public List<string> pokemonList;

		const int maxPerPage = 48;
		const int nColumns = 6;

		public Asset<Texture2D> notSeenIcon;
		public Asset<Texture2D> betaIcon;

		public int page;

		public static bool hidden = true;

		public override void OnInitialize()
		{
			notSeenIcon = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");
			betaIcon = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/Kadabra");

			page = 0;

			PokedexPanel = new DraggableUIPanel();
			PokedexPanel.SetPadding(20);

			SetRectangleAlign(PokedexPanel, left: 0.5f, top: 0.6f, width: 980, height: 680);

			ListPanel = new UIPanel();
			ListPanel.SetPadding(20);

			SetRectangleAlign(ListPanel, left: 0f, top: 0f, width: 460, height: 640);

			InfoPanel = new UIPanel();
			InfoPanel.SetPadding(20);

			SetRectangleAlign(InfoPanel, left: 1f, top: 0f, width: 460, height: 640);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Camera_5");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
			SetRectangle(closeButton, left: PokedexPanel.Width.Pixels - PokedexPanel.PaddingLeft - PokedexPanel.PaddingRight - 32, top: 0, width: 32f, height: 32f);
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);

			PokedexPanel.Append(ListPanel);
			PokedexPanel.Append(InfoPanel);
			PokedexPanel.Append(closeButton);

			pokemonList = PokemonData.pokemonInfo.Keys.ToList();
			int maxIndex = PokemonData.GetMaxPokemonIndex();
			if (maxIndex > -1) pokemonList = pokemonList.GetRange(0, maxIndex + 1);
			
			Append(PokedexPanel);
        }

		public override void OnActivate()
		{
			ChangePage(page);
			SelectPokemon(0);
        }

		public void ChangePage(int page)
		{
			int maxpage = pokemonList.Count / maxPerPage;

			if (page < 0) page = 0;
			else if (page > (float)(pokemonList.Count / maxPerPage)) page = maxpage;

			this.page = page;

			ListPanel.RemoveAllChildren();

			for (int i = 0; i < maxPerPage; i++)
			{
				int pokeNumber = page * maxPerPage + i;
				if (pokeNumber > pokemonList.Count - 1) break;

				string PokemonName = pokemonList[pokeNumber];

				PokemonInfo info = PokemonData.pokemonInfo[PokemonName];

				Asset<Texture2D> buttonTexture = info.completed ? notSeenIcon : betaIcon;
				if (true) buttonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + PokemonName);

				UIHoverPanelImageButton pokeButton = new UIHoverPanelImageButton(buttonTexture, PokemonName);
				SetRectangle(pokeButton, left: (i % nColumns) * 70f, top: ((i - (i % nColumns)) / nColumns) * 70f, width: 64f, height: 64f);

				var buttonText = new UIText("" + info.pokemonID, 0.8f)
				{
					TextColor = Color.White,
					HAlign = 0.5f,
					VAlign = 0f,
					Width = new(48f, 0f),
					Height = new(20f, 0f),
					TextOriginX = 0,
					TextOriginY = 1f
				};

				pokeButton.OnLeftClick += (a, b) => SelectPokemon(pokeNumber);

				pokeButton.Append(buttonText);

				ListPanel.Append(pokeButton);
			}

			Asset<Texture2D> buttonPrevTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Button_Back");
			UIHoverImageButton prevButton = new UIHoverImageButton(buttonPrevTexture, Language.GetTextValue("LegacyMenu.239"));
			SetRectangle(prevButton, left: 0, top: ListPanel.Height.Pixels - ListPanel.PaddingTop - ListPanel.PaddingBottom - 32, width: 32f, height: 32f);
			prevButton.OnLeftClick += (a, b) => ChangePage(page - 1);
			ListPanel.Append(prevButton);

			Asset<Texture2D> buttonNextTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Button_Forward");
			UIHoverImageButton nextButton = new UIHoverImageButton(buttonNextTexture, Language.GetTextValue("LegacyMenu.240"));
			SetRectangle(nextButton, left: 40, top: ListPanel.Height.Pixels - ListPanel.PaddingTop - ListPanel.PaddingBottom - 32, width: 32f, height: 32f);
			nextButton.OnLeftClick += (a, b) => ChangePage(page + 1);
			ListPanel.Append(nextButton);

			var pageText = new UIText((page + 1) + " of " + (maxpage + 1)/* + " (" + (page * maxPerPage + 1) + " - " + ((page + 1) * maxPerPage)+")"*/, 0.8f)
			{
				TextColor = Color.White,
				Width = new(48f, 0f),
				Height = new(20f, 0f),
				TextOriginX = 0,
				TextOriginY = 0.5f,
				Left = new(80f, 0f),
				Top = new(ListPanel.Height.Pixels - ListPanel.PaddingTop - ListPanel.PaddingBottom - 32, 0f)
			};
			ListPanel.Append(pageText);
		}

		public void SelectPokemon(int index)
		{
			InfoPanel.RemoveAllChildren();

			UIPanel titlePanel = new UIPanel();
			SetRectangle(titlePanel, left: 0f, top: 0f, width: 420, height: 40);

			var titleText = new UIText(pokemonList[index], 1f)
			{
				TextColor = Color.White,
				Width = new(400f, 0f),
				Height = new(100f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};

			titlePanel.Append(titleText);
			InfoPanel.Append(titlePanel);

			UIPanel imagePanel = new UIPanel();
			SetRectangle(imagePanel, left: 0f, top: 50f, width: 420, height: 300);

			InfoPanel.Append(imagePanel);
		}

		private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
		{
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
		
		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<PokedexUISystem>().HideMyUI();
		}
	}
}