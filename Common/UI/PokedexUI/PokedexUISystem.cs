using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokedexUI
{
	public class PokedexUISystem : ModSystem
	{
		private UserInterface PokedexUserInterface;
		internal PokedexUIState PokedexUI;
        
		// These two methods will set the state of our custom UI, causing it to show or hide
		public void ShowMyUI() {
			PokedexUIState.hidden = false;
            PokedexUserInterface?.SetState(PokedexUI);
		}
		
		public void HideMyUI() {
            PokedexUIState.hidden = true;
            PokedexUserInterface?.SetState(null);
		}

		public bool IsActive()
		{
			return PokedexUserInterface?.CurrentState != null;
		}

        public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				// Create custom interface which can swap between different UIStates
				PokedexUserInterface = new UserInterface();
				// Creating custom UIState
				PokedexUI = new PokedexUIState();

				// Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				PokedexUI.Activate();
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
			if (PokedexUserInterface?.CurrentState != null){
				PokedexUserInterface?.Update(gameTime);
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
					"Pokemod: Pokedex",
					delegate {
						if (PokedexUserInterface?.CurrentState != null)
							PokedexUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}