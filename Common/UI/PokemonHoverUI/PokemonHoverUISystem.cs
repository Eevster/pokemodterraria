using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonHoverUI
{
    public class PokemonHoverUISystem : ModSystem
    {
        private UserInterface PokemonHoverInterface;
        internal PokemonHoverUI PokemonHoverUI;

        public override void Load()
        {
            if (Main.dedServ) return;
            PokemonHoverUI = new PokemonHoverUI();
            PokemonHoverUI.Activate();
            PokemonHoverInterface = new UserInterface();
            PokemonHoverInterface?.SetState(PokemonHoverUI);
        }

        public override void Unload()
        {
            if (Main.dedServ) return;
            PokemonHoverInterface = null;
            PokemonHoverUI = null;
        }

        public override void UpdateUI(GameTime gameTime) {
            if (PokemonHoverInterface?.CurrentState != null){
                PokemonHoverInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (index != -1) {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Pokemod: Pokemon Hover UI",
                    delegate {
                        if (PokemonHoverInterface?.CurrentState != null)
                            PokemonHoverInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}