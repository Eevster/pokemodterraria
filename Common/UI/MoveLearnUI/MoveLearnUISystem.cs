using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.MoveLearnUI
{
	public class MoveLearnUISystem : ModSystem
	{
		private UserInterface MoveLearnUserInterface;
		internal MoveLearnUIState MoveLearnUI;
        
		// These two methods will set the state of our custom UI, causing it to show or hide
		public void ShowMyUI(CaughtPokemonItem pokemon, string newMove, int itemUsedType = -1, int itemUsedAmount = 1) {
            MoveLearnUI.SetMoveData(pokemon, newMove, itemUsedType, itemUsedAmount);
			MoveLearnUIState.hidden = false;
            MoveLearnUserInterface?.SetState(MoveLearnUI);
		}
		
		public void HideMyUI() {
            MoveLearnUIState.hidden = true;
            MoveLearnUserInterface?.SetState(null);
		}

		public void CatchUIState(MoveLearnUIState state)
		{
            MoveLearnUIState.hidden = false;
            MoveLearnUserInterface?.SetState(state);
        }

		public bool IsActive()
		{
			return MoveLearnUserInterface?.CurrentState != null;
		}

        public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				// Create custom interface which can swap between different UIStates
				MoveLearnUserInterface = new UserInterface();
				// Creating custom UIState
				MoveLearnUI = new MoveLearnUIState();

				// Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				MoveLearnUI.Activate();
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
			if (MoveLearnUserInterface?.CurrentState != null){
				MoveLearnUserInterface?.Update(gameTime);
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
					"Pokemod: Move Learn",
					delegate {
						if (MoveLearnUserInterface?.CurrentState != null)
							MoveLearnUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}