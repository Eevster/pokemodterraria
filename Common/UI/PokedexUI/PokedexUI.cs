using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ModLoader.UI;

namespace Pokemod.Common.UI.PokedexUI
{
	public class PokedexUIState : UIState
	{
		public DraggableUIPanel PokedexPanel;

		public UIPanel ListPanel;
		public UIPanel InfoPanel;

		public UIPokedexInfoDisplay pokeInfoDisplay;

		public List<string> pokemonAuxList;
		public List<string> pokemonList;

		const int maxPerPage = 48;
		const int nColumns = 6;

		public Asset<Texture2D> notSeenIcon;

		public int page;

		public static bool hidden = true;

		public override void OnInitialize()
		{
			notSeenIcon = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");

			page = 0;

			PokedexPanel = new DraggableUIPanel();
			PokedexPanel.SetPadding(20);

			UIHelpers.SetRectangleAlign(PokedexPanel, left: 0.5f, top: 0.6f, width: 980, height: 680);

			ListPanel = new UIPanel();
			ListPanel.SetPadding(20);

			UIHelpers.SetRectangleAlign(ListPanel, left: 0f, top: 0f, width: 460, height: 640);

			InfoPanel = new UIPanel();
			InfoPanel.SetPadding(20);

			UIHelpers.SetRectangleAlign(InfoPanel, left: 1f, top: 0f, width: 460, height: 640);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Camera_5");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
			UIHelpers.SetRectangle(closeButton, left: PokedexPanel.Width.Pixels - PokedexPanel.PaddingLeft - PokedexPanel.PaddingRight - 32, top: 0, width: 32f, height: 32f);
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);

			PokedexPanel.Append(ListPanel);
			PokedexPanel.Append(InfoPanel);
			PokedexPanel.Append(closeButton);

			pokemonAuxList = PokemonData.pokemonInfo.Keys.ToList();

			int maxIndex = PokemonData.GetMaxPokemonIndex();
			if (maxIndex > -1) pokemonAuxList = pokemonAuxList.GetRange(0, maxIndex + 1);

			if (!ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle)
			{
				pokemonAuxList = pokemonAuxList.Where(x => PokemonData.pokemonInfo[x].completed).ToList();
			}

			pokemonList = pokemonAuxList;

			Append(PokedexPanel);
		}

		public override void OnActivate()
		{
			ChangePage(page);
		}

		public override void OnDeactivate()
		{
			InfoPanel.RemoveAllChildren();
		}

		public void ChangePage(int page)
		{
			int maxpage = pokemonList.Count / maxPerPage;

			if (page < 0) page = 0;
			else if (page > (float)(pokemonList.Count / maxPerPage)) page = maxpage;

			this.page = page;

			ListPanel.RemoveAllChildren();

			if (Main.player[Main.myPlayer].TryGetModPlayer(out PokemonPlayer pkPlayer))
			{
				for (int i = 0; i < maxPerPage; i++)
				{
					int pokeNumber = page * maxPerPage + i;
					if (pokeNumber > pokemonList.Count - 1) break;

					string PokemonName = pokemonList[pokeNumber];

					PokemonInfo info = PokemonData.pokemonInfo[PokemonName];

					Asset<Texture2D> buttonTexture = notSeenIcon;
					UIHoverPanelImageButton pokeButton = new UIHoverPanelImageButton(buttonTexture, "???");

					int phase = 0;

					if (pkPlayer.registeredPokemon.Keys.ToList().Contains(PokemonName))
					{
						buttonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + PokemonName);
						pokeButton = new UIHoverPanelImageButton(buttonTexture, Language.GetTextValue("Mods.Pokemod.NPCs." + PokemonName + "CritterNPC.DisplayName"));
						if (pkPlayer.registeredPokemon[PokemonName] <= 0)
						{
							pokeButton.ButtonColor = Color.Black;
							phase = 1;
						}
						else phase = 2;
					}

					UIHelpers.SetRectangle(pokeButton, left: (i % nColumns) * 70f, top: ((i - (i % nColumns)) / nColumns) * 70f, width: 64f, height: 64f);

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

					pokeButton.OnLeftClick += (a, b) => SelectPokemon(pokeNumber, phase);

					pokeButton.Append(buttonText);

					ListPanel.Append(pokeButton);
				}
			}

			Asset<Texture2D> buttonPrevTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Button_Back");
			UIHoverImageButton prevButton = new UIHoverImageButton(buttonPrevTexture, Language.GetTextValue("LegacyMenu.239"));
			UIHelpers.SetRectangle(prevButton, left: 0, top: ListPanel.Height.Pixels - ListPanel.PaddingTop - ListPanel.PaddingBottom - 32, width: 32f, height: 32f);
			prevButton.OnLeftClick += (a, b) => ChangePage(page - 1);
			ListPanel.Append(prevButton);

			Asset<Texture2D> buttonNextTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Button_Forward");
			UIHoverImageButton nextButton = new UIHoverImageButton(buttonNextTexture, Language.GetTextValue("LegacyMenu.240"));
			UIHelpers.SetRectangle(nextButton, left: 40, top: ListPanel.Height.Pixels - ListPanel.PaddingTop - ListPanel.PaddingBottom - 32, width: 32f, height: 32f);
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

			//SoundEngine.PlaySound(SoundID.MenuOpen);
		}

		public void SelectPokemon(int index, int phase = 0)
		{
			InfoPanel.RemoveAllChildren();

			UIPanel titlePanel = new UIPanel();
			UIHelpers.SetRectangle(titlePanel, left: 0f, top: 0f, width: 420, height: 40);

			string pokemonName = pokemonList[index];

			var titleText = new UIText(phase == 0 ? "???" : Language.GetTextValue("Mods.Pokemod.NPCs." + pokemonName + "CritterNPC.DisplayName"), 1f)
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
			UIHelpers.SetRectangle(imagePanel, left: 0f, top: 50f, width: 365, height: 215);
			imagePanel.HAlign = 0.5f;

			Asset<Texture2D> BGTexture = ModContent.Request<Texture2D>("Terraria/Images/MapBG1");

			UIImage imageFrame = new UIImage(BGTexture)
			{
				ScaleToFit = true
			};
			UIHelpers.SetRectangleAlign(imageFrame, left: 0.5f, top: 0.5f, width: 345, height: 195);

			if (phase == 0)
			{
				UIImage pokeImage = new UIImage(notSeenIcon)
				{
					ImageScale = 2f,
					NormalizedOrigin = new Vector2(0.5f, 0.5f)
				};
				UIHelpers.SetRectangleAlign(pokeImage, left: 0.5f, top: 0.5f, width: 0, height: 0);

				imageFrame.Append(pokeImage);
			}
			else
			{
				if (ModContent.TryFind<ModNPC>("Pokemod", pokemonName + "CritterNPC", out var npcBase))
				{
					if (ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Pets/" + pokemonName + "PetProjectile", out Asset<Texture2D> pokeTexture))
					{
						PokemonWildNPC npc = (PokemonWildNPC)npcBase;
						UIAnimImage pokeImage = new UIAnimImage(pokeTexture, npc.totalFrames, npc.walkStartEnd[0] != -1 ? npc.walkStartEnd[0] : npc.idleStartEnd[0], npc.walkStartEnd[1] != -1 ? npc.walkStartEnd[1] : npc.idleStartEnd[1])
						{
							frameRate = npc.animationSpeed,
							Color = phase == 1 ? Color.Black : Color.White,
							ImageScale = 2f
						};
						UIHelpers.SetRectangleAlign(pokeImage, left: 0.5f, top: 0.5f, width: 345, height: 195);

						imageFrame.Append(pokeImage);

						pokeInfoDisplay = new UIPokedexInfoDisplay(npc);
						UIHelpers.SetRectangle(pokeInfoDisplay, left: 0f, top: 275f, width: 420, height: 320);
						pokeInfoDisplay.SetPadding(0);

						InfoPanel.Append(pokeInfoDisplay);
					}
				}
			}

			imagePanel.Append(imageFrame);

			InfoPanel.Append(imagePanel);

			//SoundEngine.PlaySound(SoundID.MenuOpen);
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<PokedexUISystem>().HideMyUI();
		}
	}
	
	public class UIPokedexInfoDisplay : UIElement
	{
		public UIElement container;

		public UIPanel infoPanel;
		public UIPanel areaPanel;
		public UIPanel formsPanel;

		public UIPokedexInfoDisplay(PokemonWildNPC npc)
		{
			SetInfo(npc);
			SetInitialContent();
			SetContent(infoPanel);
		}

		private void SetInitialContent()
		{
			UIButton<string> infoButton = new("Info");

			UIHelpers.SetRectangle(infoButton, left: 0f, top: 0f, width: 100, height: 40);
			var infoText = new UIText("Info", 1f)
			{
				TextColor = Color.White,
				Width = new(80f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			infoButton.Append(infoText);
			infoButton.OnLeftClick += (a, b) => SetContent(infoPanel);

			UIButton<string> areaButton = new("Area");
			UIHelpers.SetRectangle(areaButton, left: 100f, top: 0f, width: 100, height: 40);
			var areaText = new UIText("Area", 1f)
			{
				TextColor = Color.White,
				Width = new(80f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			areaButton.Append(areaText);
			areaButton.OnLeftClick += (a, b) => SetContent(areaPanel);

			UIButton<string> formsButton = new("Forms");
			UIHelpers.SetRectangle(formsButton, left: 200f, top: 0f, width: 100, height: 40);
			var formsText = new UIText("Forms", 1f)
			{
				TextColor = Color.White,
				Width = new(80f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			formsButton.Append(formsText);
			formsButton.OnLeftClick += (a, b) => SetContent(formsPanel);

			container = new UIElement();
			UIHelpers.SetRectangle(container, left: 0f, top: 40f, width: 420, height: 280);
			container.SetPadding(0);

			Append(infoButton);
			Append(areaButton);
			Append(formsButton);
			Append(container);
		}

		public void SetContent(UIPanel contentPanel)
		{
			container.RemoveAllChildren();
			container.Append(contentPanel);
		}

		public void SetInfo(PokemonWildNPC npc)
		{
			infoPanel = new UIPanel();
			UIHelpers.SetRectangle(infoPanel, left: 0f, top: 0f, width: 420, height: 280);
			infoPanel.SetPadding(20);
			var descText = new UIText(Language.GetTextValue("Mods.Pokemod.NPCs." + npc.pokemonName + "CritterNPC.Desc"), 1f)
			{
				TextColor = Color.White,
				Width = new(380f, 0f),
				Height = new(220f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0f,
			};
			infoPanel.Append(descText);

			areaPanel = new UIPanel();
			UIHelpers.SetRectangle(areaPanel, left: 0f, top: 0f, width: 420, height: 280);
			areaPanel.SetPadding(20);
			var areaText = new UIText("Unknown", 1f)
			{
				TextColor = Color.White,
				Width = new(380f, 0f),
				Height = new(220f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0f,
			};
			areaPanel.Append(areaText);

			formsPanel = new UIPanel();
			UIHelpers.SetRectangle(formsPanel, left: 0f, top: 0f, width: 420, height: 280);
			formsPanel.SetPadding(20);
			var formsText = new UIText("No alternative forms", 1f)
			{
				TextColor = Color.White,
				Width = new(380f, 0f),
				Height = new(220f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0f,
			};
			formsPanel.Append(formsText);
		}
	}
}