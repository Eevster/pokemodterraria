using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.MoveTutorUI
{
	public class MoveTutorUISystem : ModSystem
	{
		private UserInterface MoveTutorUserInterface;
		internal MoveTutorUIState MoveTutorUI;
		private NPC refNPC;
        
		// These two methods will set the state of our custom UI, causing it to show or hide
		public void ShowMyUI(NPC triggerNPC) {
			refNPC = triggerNPC;
			Main.playerInventory = true;
            Main.hidePlayerCraftingMenu = true;
            MoveTutorUI.OpenPanel();
            MoveTutorUserInterface?.SetState(MoveTutorUI);
		}
		
		public void HideMyUI() 
		{
            Main.hidePlayerCraftingMenu = false;
            MoveTutorUI.ClosePanel();
            MoveTutorUserInterface?.SetState(null);
		}

		public void RefreshUI()
		{
            if (MoveTutorUserInterface?.CurrentState != null)
			{
				MoveTutorUI.SetPokemon();
			}
        }

		public bool IsActive()
		{
			return MoveTutorUserInterface?.CurrentState != null;
		}

        public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				// Create custom interface which can swap between different UIStates
				MoveTutorUserInterface = new UserInterface();
				// Creating custom UIState
				MoveTutorUI = new MoveTutorUIState();
				// Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				MoveTutorUI.Activate();
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
			if (MoveTutorUserInterface?.CurrentState != null){
                Main.hidePlayerCraftingMenu = true;
                MoveTutorUserInterface?.Update(gameTime);
				if(Main.LocalPlayer.controlInv || Main.LocalPlayer.chest != -1 || Main.LocalPlayer.TalkNPC != refNPC)
				{
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
						if (MoveTutorUserInterface?.CurrentState != null)
							MoveTutorUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}