using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.StarterPanelUI
{
	public class StarterPanelUISystem : ModSystem
	{
		private UserInterface StarterPanelUserInterface;
		internal StarterPanelUIState StarterPanelUI;
        
		// These two methods will set the state of our custom UI, causing it to show or hide
		public void ShowMyUI() {
			StarterPanelUserInterface?.SetState(StarterPanelUI);
		}
		
		public void HideMyUI() {
			StarterPanelUserInterface?.SetState(null);
		}

        public override void PostSetupContent()
        {
			if (!Main.dedServ)
			{
				// Create custom interface which can swap between different UIStates
				StarterPanelUserInterface = new UserInterface();
				// Creating custom UIState
				StarterPanelUI = new StarterPanelUIState();

				// Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				StarterPanelUI.Activate();
			}
        }

		public override void UpdateUI(GameTime gameTime) {
			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
			if (StarterPanelUserInterface?.CurrentState != null){
				StarterPanelUserInterface?.Update(gameTime);
				if(Main.LocalPlayer.controlInv){
					HideMyUI();
				}
			}
		}

		// Adding a custom layer to the vanilla layer list that will call .Draw on your interface if it has a state
		// Setting the InterfaceScaleType to UI for appropriate UI scaling
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"Pokemod: Starter Panel",
					delegate {
						if (StarterPanelUserInterface?.CurrentState != null)
							StarterPanelUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}