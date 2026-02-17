using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.GloveAreaUI
{
    public class GloveUISystem : ModSystem
    {
        private UserInterface PokemonGloveInterface;
        internal GloveUI PokemonGloveUI;

        public override void Load()
        {
            if (Main.dedServ) return;
            PokemonGloveUI = new GloveUI();
            PokemonGloveUI.Activate();
            PokemonGloveInterface = new UserInterface();
            PokemonGloveInterface?.SetState(PokemonGloveUI);
        }

        public override void Unload()
        {
            if (Main.dedServ) return;
            PokemonGloveInterface = null;
            PokemonGloveUI = null;
        }

        public override void UpdateUI(GameTime gameTime) {
            if (PokemonGloveInterface?.CurrentState != null){
                PokemonGloveInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (index != -1) {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Pokemod: Glove UI",
                    delegate {
                        if (PokemonGloveInterface?.CurrentState != null)
                            PokemonGloveInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}