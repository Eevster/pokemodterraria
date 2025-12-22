using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonCursorUI
{
    public class CursorUISystem : ModSystem
    {
        private UserInterface PokemonCursorInterface;
        internal CursorUI PokemonCursorUI;

        public override void Load()
        {
            if (Main.dedServ) return;
            PokemonCursorUI = new CursorUI();
            PokemonCursorUI.Activate();
            PokemonCursorInterface = new UserInterface();
            PokemonCursorInterface?.SetState(PokemonCursorUI);
        }

        public override void Unload()
        {
            if (Main.dedServ) return;
            PokemonCursorInterface = null;
            PokemonCursorUI = null;
        }

        public override void UpdateUI(GameTime gameTime) {
            if (PokemonCursorInterface?.CurrentState != null){
                PokemonCursorInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (index != -1) {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Pokemod: Cursor UI",
                    delegate {
                        if (PokemonCursorInterface?.CurrentState != null)
                            PokemonCursorInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}