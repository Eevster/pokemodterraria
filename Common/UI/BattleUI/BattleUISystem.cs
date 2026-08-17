using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.BattleUI
{
    public class BattleUISystem : ModSystem
    {
        private UserInterface PokemonBattleInterface;
        internal BattleUI PokemonBattleUI;

        public void ShowMyUI() {
            PokemonBattleInterface?.SetState(PokemonBattleUI);
		}
		
		public void HideMyUI() {
            PokemonBattleInterface?.SetState(null);
		}

        public bool IsActive()
		{
			return PokemonBattleInterface?.CurrentState != null;
		}

        public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				PokemonBattleInterface = new UserInterface();
				PokemonBattleUI = new BattleUI();

				PokemonBattleUI.Activate();
			}
		}

        public override void PreSaveAndQuit()
        {
            base.PreSaveAndQuit();
            HideMyUI();
        }

		public override void UpdateUI(GameTime gameTime) {
			if (PokemonBattleInterface?.CurrentState != null){
                if (Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().onBattle)
                {
                    PokemonBattleInterface?.Update(gameTime);
                }
                else
                {
                    HideMyUI();
                }
			}
		}

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (index != -1) {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Pokemod: Battle UI",
                    delegate {
                        if (PokemonBattleInterface?.CurrentState != null)
                            PokemonBattleInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}