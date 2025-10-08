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
using Microsoft.Xna.Framework.Design;
using Pokemod.Content.Pets;

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
		public Asset<Texture2D> spawnIcons;

		public int page;

		public static bool hidden = true;

		public override void OnInitialize()
		{
			notSeenIcon = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");
			spawnIcons = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow");

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

					if (phase == 0) pokeButton.BackgroundColor = new Color(21, 27, 50) * 0.7f;

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

					pokeButton.OnLeftClick += (a, b) => SelectPokemon(PokemonName, phase);

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

		public void SelectPokemon(string pokemonName, int phase = 0)
		{
			InfoPanel.RemoveAllChildren();

			UIPanel titlePanel = new UIPanel();
			UIHelpers.SetRectangle(titlePanel, left: 0f, top: 0f, width: 420, height: 40);

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
				PokemonWildNPC npc = null;

				if (ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Pets/" + pokemonName + "PetProjectile", out Asset<Texture2D> pokeTexture))
				{
					if (ModContent.TryFind<ModNPC>("Pokemod", pokemonName + "CritterNPC", out var npcBase))
					{
						npc = (PokemonWildNPC)npcBase;
						UIAnimImage pokeImage = new UIAnimImage(pokeTexture, npc.totalFrames, npc.walkStartEnd[0] != -1 ? npc.walkStartEnd[0] : npc.idleStartEnd[0], npc.walkStartEnd[1] != -1 ? npc.walkStartEnd[1] : npc.idleStartEnd[1])
						{
							frameRate = npc.animationSpeed,
							Color = phase == 1 ? Color.Black : Color.White,
							ImageScale = 2f
						};
						UIHelpers.SetRectangleAlign(pokeImage, left: 0.5f, top: 0.5f, width: 345, height: 195);

						imageFrame.Append(pokeImage);
					}
					else if (ModContent.TryFind<ModProjectile>("Pokemod", pokemonName + "PetProjectile", out var projBase))
					{
						PokemonPetProjectile proj = (PokemonPetProjectile)projBase;
						UIAnimImage pokeImage = new UIAnimImage(pokeTexture, proj.totalFrames, proj.walkStartEnd[0] != -1 ? proj.walkStartEnd[0] : proj.idleStartEnd[0], proj.walkStartEnd[1] != -1 ? proj.walkStartEnd[1] : proj.idleStartEnd[1])
						{
							frameRate = proj.animationSpeed,
							Color = phase == 1 ? Color.Black : Color.White,
							ImageScale = 2f
						};
						UIHelpers.SetRectangleAlign(pokeImage, left: 0.5f, top: 0.5f, width: 345, height: 195);

						imageFrame.Append(pokeImage);
					}

					pokeInfoDisplay = new UIPokedexInfoDisplay(pokemonName, phase == 2);
					UIHelpers.SetRectangle(pokeInfoDisplay, left: 0f, top: 275f, width: 420, height: 320);
					pokeInfoDisplay.SetPadding(0);

					pokeInfoDisplay.areaPanel.RemoveAllChildren();

					if (npc != null)
					{
						if (npc.spawnConditions.Length == 0)
						{
							var areaText = new UIText("???", 0.8f)
							{
								TextColor = Color.White,
								Width = new(360f, 0f),
								Height = new(30f, 0f),
								TextOriginX = 0.5f,
								TextOriginY = 0.5f,
							};

							pokeInfoDisplay.areaPanel.Append(areaText);
						}

						for (int i = 0; i < npc.spawnConditions.Length; i++)
						{
							if (npc.spawnConditions[i].Length > 0){
								UIPanel biomePanel = new UIPanel();
								UIHelpers.SetRectangle(biomePanel, left: 0f, top: i * 40f, width: 126f, height: 40f);
								biomePanel.HAlign = 0f;
								biomePanel.SetPadding(0);

								UIImage biomeImage = new UIImageSection(spawnIcons, 16, 5, npc.spawnConditions[i][0] % 16, (int)(npc.spawnConditions[i][0] / 16));
								UIHelpers.SetRectangleAlign(biomeImage, left: 0f, top: 0.5f, width: 32, height: 32);
								biomePanel.Append(biomeImage);

								var biomeText = new UIText(Language.GetTextValue("Mods.Pokemod.SpawnArea." + (SpawnArea)npc.spawnConditions[i][0]), 0.7f)
								{
									TextColor = Color.White,
									Width = new(126f, 0f),
									Height = new(40f, 0f),
									TextOriginX = 0.5f,
									TextOriginY = 0.5f,
								};
								biomePanel.Append(biomeText);

								pokeInfoDisplay.areaPanel.Append(biomePanel);

								if (npc.spawnConditions[i].Length > 1)
								{
									UIPanel timePanel = new UIPanel();
									UIHelpers.SetRectangle(timePanel, left: 0f, top: i * 40f, width: 126f, height: 40f);
									timePanel.HAlign = 0.5f;
									timePanel.SetPadding(0);

									UIImage timeImage = new UIImageSection(spawnIcons, 16, 5, npc.spawnConditions[i][1] % 16, (int)(npc.spawnConditions[i][1] / 16));
									UIHelpers.SetRectangleAlign(timeImage, left: 0f, top: 0.5f, width: 32, height: 32);
									timePanel.Append(timeImage);

									var timeText = new UIText(Language.GetTextValue("Mods.Pokemod.DayTimeStatus." + (DayTimeStatus)npc.spawnConditions[i][1]), 0.7f)
									{
										TextColor = Color.White,
										Width = new(126f, 0f),
										Height = new(40f, 0f),
										TextOriginX = 0.5f,
										TextOriginY = 0.5f,
									};
									timePanel.Append(timeText);

									pokeInfoDisplay.areaPanel.Append(timePanel);
								}

								if (npc.spawnConditions[i].Length > 2) {
									UIPanel weatherPanel = new UIPanel();
									UIHelpers.SetRectangle(weatherPanel, left: 0f, top: i * 40f, width: 126f, height: 40f);
									weatherPanel.HAlign = 1f;
									weatherPanel.SetPadding(0);

									UIImage weatherImage = new UIImageSection(spawnIcons, 16, 5, npc.spawnConditions[i][2] % 16, (int)(npc.spawnConditions[i][2] / 16));
									UIHelpers.SetRectangleAlign(weatherImage, left: 0f, top: 0.5f, width: 32, height: 32);
									weatherPanel.Append(weatherImage);

									var weatherText = new UIText(Language.GetTextValue("Mods.Pokemod.WeatherStatus." + (WeatherStatus)npc.spawnConditions[i][2]), 0.7f)
									{
										TextColor = Color.White,
										Width = new(126f, 0f),
										Height = new(40f, 0f),
										TextOriginX = 0.5f,
										TextOriginY = 0.5f,
									};
									weatherPanel.Append(weatherText);

									pokeInfoDisplay.areaPanel.Append(weatherPanel);
								}
							}
						}
					}

					pokeInfoDisplay.formsPanel.RemoveAllChildren();

					if (phase != 0)
					{
						List<string> nForms = PokemonData.GetAllForms(pokemonName);

						for (int i = 0; i < nForms.Count; i++)
						{
							string formName = nForms[i];

							int altPhase = 1;

							if (Main.player[Main.myPlayer].TryGetModPlayer(out PokemonPlayer pkPlayer))
							{
								if (pkPlayer.registeredPokemon.Keys.ToList().Contains(formName))
								{
									if (pkPlayer.registeredPokemon[formName] > 0) altPhase = 2;
								}
							}

							UIButton<string> formButton = new(Language.GetTextValue("Mods.Pokemod.NPCs." + formName + "CritterNPC.DisplayName"));
							UIHelpers.SetRectangle(formButton, left: 0f, top: 45f * i, width: 360, height: 40);

							formButton.OnLeftClick += (a, b) => SelectPokemon(formName, altPhase);

							pokeInfoDisplay.formsPanel.Append(formButton);
						}
					}

					InfoPanel.Append(pokeInfoDisplay);
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

		public UIPokedexInfoDisplay(string pokemonName, bool captured = true)
		{
			SetInfo(pokemonName, captured);
			SetInitialContent();
			SetContent(infoPanel);
		}

		private void SetInitialContent()
		{
			UIButton<string> infoButton = new("Info");
			UIHelpers.SetRectangle(infoButton, left: 0f, top: 0f, width: 100, height: 40);
			infoButton.OnLeftClick += (a, b) => SetContent(infoPanel);

			UIButton<string> areaButton = new("Area");
			UIHelpers.SetRectangle(areaButton, left: 100f, top: 0f, width: 100, height: 40);
			areaButton.OnLeftClick += (a, b) => SetContent(areaPanel);

			UIButton<string> formsButton = new("Forms");
			UIHelpers.SetRectangle(formsButton, left: 200f, top: 0f, width: 100, height: 40);
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

		public void SetInfo(string pokemonName, bool captured = true)
		{
			//INFO CONTENT
			infoPanel = new UIPanel();
			UIHelpers.SetRectangle(infoPanel, left: 0f, top: 0f, width: 420, height: 280);
			infoPanel.SetPadding(20);
			var descText = new UIText(captured?Language.GetTextValue("Mods.Pokemod.NPCs." + pokemonName + "CritterNPC.Description"):"?????", 0.9f)
			{
				IsWrapped = true,
				TextColor = Color.White,
				Width = new(380f, 0f),
				Height = new(80f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0f,
			};
			infoPanel.Append(descText);
			
			UIHorizontalSeparator separator = new UIHorizontalSeparator(1) {Color = new Color(0f,0f,0f,0.5f)};
			UIHelpers.SetRectangle(separator, left: 0f, top: 80f, width: 420, height: 1);
			infoPanel.Append(separator);

			UIElement typeContainer = new UIElement();
			UIHelpers.SetRectangle(typeContainer, left: 0f, top: 80f, width: 180, height: 40);
			typeContainer.SetPadding(0);

			PokemonInfo pokeInfo = PokemonData.pokemonInfo[pokemonName];

			if (pokeInfo.pokemonTypes[0] != -1 && pokeInfo.pokemonTypes[1] != -1)
			{
				UIPanel typePanel = new UIPanel()
				{
					BackgroundColor = ColorConverter.HexToXnaColor(PokemonNPCData.GetTypeColor(pokeInfo.pokemonTypes[0]))
				};
				UIHelpers.SetRectangle(typePanel, left: 0.5f * typeContainer.Width.Pixels - 80 - 5, top: 0, width: 80, height: 30);
				typePanel.VAlign = 0.5f;
				typePanel.SetPadding(0);

				var typeText = new UIText(Language.GetTextValue("Mods.Pokemod.PokemonTypes." + (TypeIndex)pokeInfo.pokemonTypes[0]), 0.8f)
				{
					TextColor = Color.White,
					Width = new(80f, 0f),
					Height = new(30f, 0f),
					TextOriginX = 0.5f,
					TextOriginY = 0.5f,
				};
				typePanel.Append(typeText);
				typeContainer.Append(typePanel);

				UIPanel type2Panel = new UIPanel()
				{
					BackgroundColor = ColorConverter.HexToXnaColor(PokemonNPCData.GetTypeColor(pokeInfo.pokemonTypes[1]))
				};
				UIHelpers.SetRectangle(type2Panel, left: 0.5f * typeContainer.Width.Pixels + 5, top: 0, width: 80, height: 30);
				type2Panel.VAlign = 0.5f;
				type2Panel.SetPadding(0);

				var type2Text = new UIText(Language.GetTextValue("Mods.Pokemod.PokemonTypes." + (TypeIndex)pokeInfo.pokemonTypes[1]), 0.8f)
				{
					TextColor = Color.White,
					Width = new(80f, 0f),
					Height = new(30f, 0f),
					TextOriginX = 0.5f,
					TextOriginY = 0.5f,
				};
				type2Panel.Append(type2Text);
				typeContainer.Append(type2Panel);
			}
			else
			{
				UIPanel typePanel = new UIPanel()
				{
					BackgroundColor = pokeInfo.pokemonTypes[0] == -1 ? Color.Black : ColorConverter.HexToXnaColor(PokemonNPCData.GetTypeColor(pokeInfo.pokemonTypes[0]))
				};
				UIHelpers.SetRectangle(typePanel, left: 0.5f * typeContainer.Width.Pixels - 40, top: 0, width: 80, height: 30);
				typePanel.VAlign = 0.5f;
				typePanel.SetPadding(0);

				var typeText = new UIText(Language.GetTextValue("Mods.Pokemod.PokemonTypes." + (TypeIndex)pokeInfo.pokemonTypes[0]), 0.8f)
				{
					TextColor = Color.White,
					Width = new(80f, 0f),
					Height = new(30f, 0f),
					TextOriginX = 0.5f,
					TextOriginY = 0.5f,
				};
				typePanel.Append(typeText);
				typeContainer.Append(typePanel);
			}

			infoPanel.Append(typeContainer);

			UIHorizontalSeparator separator2 = new UIHorizontalSeparator(1) {Color = new Color(0f,0f,0f,0.5f)};
			UIHelpers.SetRectangle(separator2, left: 0f, top: 120f, width: 180, height: 1);
			infoPanel.Append(separator2);

			var heightText = new UIText(pokeInfo.height + " m", 0.8f)
			{
				Top = new(120f,0),
				TextColor = Color.White,
				Width = new(180f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			infoPanel.Append(heightText);

			UIHorizontalSeparator separator3 = new UIHorizontalSeparator(1) {Color = new Color(0f,0f,0f,0.5f)};
			UIHelpers.SetRectangle(separator3, left: 0f, top: 160f, width: 180, height: 1);
			infoPanel.Append(separator3);

			var weightText = new UIText(pokeInfo.weight + " kg", 0.8f)
			{
				Top = new(160f,0),
				TextColor = Color.White,
				Width = new(180f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			infoPanel.Append(weightText);

			UIHorizontalSeparator separator4 = new UIHorizontalSeparator(1) {Color = new Color(0f,0f,0f,0.5f)};
			UIHelpers.SetRectangle(separator4, left: 0f, top: 200f, width: 180, height: 1);
			infoPanel.Append(separator4);

			string eggGroupTextContent = "";
			if (pokeInfo.eggGroups.Length > 0)
			{
				eggGroupTextContent += Language.GetTextValue("Mods.Pokemod.PokemonEggGroups." + (EggGroups)pokeInfo.eggGroups[0]);
				if(pokeInfo.eggGroups.Length > 1) eggGroupTextContent += " - " + Language.GetTextValue("Mods.Pokemod.PokemonEggGroups." + (EggGroups)pokeInfo.eggGroups[1]);
			}

			var eggGroupText = new UIText(eggGroupTextContent, 0.8f)
			{
				Top = new(200f,0),
				TextColor = Color.White,
				Width = new(180f, 0f),
				Height = new(40f, 0f),
				TextOriginX = 0.5f,
				TextOriginY = 0.5f,
			};
			infoPanel.Append(eggGroupText);

			for (int i = 0; i < 6; i++)
			{
				ColorBarUIPanel statPanel = new ColorBarUIPanel(captured ? (pokeInfo.pokemonStats[i]/255f):0f) {BarColor = ColorConverter.HexToXnaColor(PokemonNPCData.GetStatColor(i))};
				UIHelpers.SetRectangle(statPanel, left: 0.5f * infoPanel.Width.Pixels - infoPanel.PaddingLeft + 10, top: infoPanel.Height.Pixels - 2 * infoPanel.PaddingTop - 25 * (6 - i), width: 180, height: 25);
				statPanel.SetPadding(0);

				var statText = new UIText((StatName)i + ": " + (captured ? pokeInfo.pokemonStats[i] : "???"), 0.8f)
				{
					TextColor = Color.White,
					Width = new(180f, 0f),
					Height = new(25f, 0f),
					TextOriginX = 0.1f,
					TextOriginY = 0.5f,
				};
				statPanel.Append(statText);

				infoPanel.Append(statPanel);
			}

			//AREA CONTENT
			areaPanel = new UIPanel();
			UIHelpers.SetRectangle(areaPanel, left: 0f, top: 0f, width: 420, height: 280);
			areaPanel.SetPadding(20);
			
			//FORMS CONTENT
			formsPanel = new UIPanel();
			UIHelpers.SetRectangle(formsPanel, left: 0f, top: 0f, width: 420, height: 280);
			formsPanel.SetPadding(20);
		}
	}
}