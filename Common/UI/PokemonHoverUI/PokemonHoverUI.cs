using Terraria;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonHoverUI
{
    public class PokemonHoverUI : UIState
    {
        public override void OnInitialize()
        {
            var pokemonHoverElement = new PokemonHoverUIElement();
            pokemonHoverElement.Width.Set(Main.screenWidth, 0);
            pokemonHoverElement.Height.Set(Main.screenHeight, 0);
            Append(pokemonHoverElement);
        }
    }
}